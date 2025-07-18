using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LookingGlassController))]
public class LookingGlassBreaker : MonoBehaviour
{
    [System.Serializable]
    public class BreakSettings
    {
        [Header("Break Detection")]
        public float breakForce = 10f;
        public float breakRadius = 2f;
        public LayerMask breakableLayers = -1;
        public string breakTag = "Breakable";
        
        [Header("Fracture Settings")]
        public GameObject fractureParticlePrefab;
        public int fractureCount = 20;
        public float fractureLifetime = 5f;
        public float fractureForceMultiplier = 1f;
        
        [Header("Audio")]
        public AudioClip breakSound;
        public float breakVolume = 1f;
        
        [Header("Effects")]
        public GameObject breakEffectPrefab;
        public float effectDuration = 2f;
        public AnimationCurve breakTransition = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
        
        [Header("Repair")]
        public bool canRepair = true;
        public float repairTime = 3f;
        public float repairRadius = 1f;
        public AudioClip repairSound;
    }
    
    [SerializeField] private BreakSettings settings = new BreakSettings();
    
    private LookingGlassController glassController;
    private MeshRenderer meshRenderer;
    private Collider glassCollider;
    private AudioSource audioSource;
    private Material originalMaterial;
    private Material brokenMaterial;
    
    private bool isBroken = false;
    private bool isRepairing = false;
    private List<GameObject> fractureParticles = new List<GameObject>();
    private Coroutine breakEffectCoroutine;
    private Coroutine repairCoroutine;
    
    // Events
    public System.Action<Vector3, float> OnGlassBreak;
    public System.Action OnGlassRepair;
    
    // Shader properties for break effect
    private static readonly int BreakProgress = Shader.PropertyToID("_BreakProgress");
    private static readonly int BreakCenter = Shader.PropertyToID("_BreakCenter");
    private static readonly int BreakRadius = Shader.PropertyToID("_BreakRadius");
    
    public bool IsBroken => isBroken;
    public bool IsRepairing => isRepairing;
    public BreakSettings Settings => settings;
    
    private void Awake()
    {
        InitializeComponents();
        CreateBrokenMaterial();
    }
    
    private void InitializeComponents()
    {
        glassController = GetComponent<LookingGlassController>();
        meshRenderer = GetComponent<MeshRenderer>();
        glassCollider = GetComponent<Collider>();
        
        // Create audio source for break/repair sounds
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D sound
        
        originalMaterial = meshRenderer.material;
        
        if (glassCollider == null)
        {
            Debug.LogWarning("LookingGlassBreaker: No collider found. Adding BoxCollider.");
            glassCollider = gameObject.AddComponent<BoxCollider>();
        }
        
        // Make sure it's set as trigger for break detection
        glassCollider.isTrigger = true;
    }
    
    private void CreateBrokenMaterial()
    {
        if (originalMaterial != null)
        {
            brokenMaterial = new Material(originalMaterial);
            brokenMaterial.name = originalMaterial.name + "_Broken";
            
            // Add properties for break effect
            brokenMaterial.SetFloat(BreakProgress, 0f);
            brokenMaterial.SetVector(BreakCenter, Vector3.zero);
            brokenMaterial.SetFloat(BreakRadius, settings.breakRadius);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isBroken || isRepairing) return;
        
        // Check if the object can break the glass
        if (CanBreakGlass(other))
        {
            Vector3 impactPoint = other.ClosestPoint(transform.position);
            float impactForce = CalculateImpactForce(other);
            
            if (impactForce >= settings.breakForce)
            {
                BreakGlass(impactPoint, impactForce);
            }
        }
    }
    
    private bool CanBreakGlass(Collider other)
    {
        // Check layer mask
        if ((settings.breakableLayers & (1 << other.gameObject.layer)) == 0)
            return false;
        
        // Check tag if specified
        if (!string.IsNullOrEmpty(settings.breakTag) && !other.CompareTag(settings.breakTag))
            return false;
        
        return true;
    }
    
    private float CalculateImpactForce(Collider other)
    {
        // Try to get rigidbody for velocity-based calculation
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float velocity = rb.velocity.magnitude;
            float mass = rb.mass;
            return velocity * mass;
        }
        
        // Fallback to distance-based calculation
        float distance = Vector3.Distance(other.transform.position, transform.position);
        return Mathf.Max(0f, settings.breakForce - distance);
    }
    
    public void BreakGlass(Vector3 impactPoint, float impactForce)
    {
        if (isBroken) return;
        
        isBroken = true;
        
        // Disable looking glass effect
        glassController.EnableEffect(false);
        
        // Start break effect
        if (breakEffectCoroutine != null)
            StopCoroutine(breakEffectCoroutine);
        breakEffectCoroutine = StartCoroutine(BreakEffect(impactPoint, impactForce));
        
        // Create fracture particles
        CreateFractureParticles(impactPoint, impactForce);
        
        // Play break sound
        PlayBreakSound();
        
        // Spawn break effect
        if (settings.breakEffectPrefab != null)
        {
            GameObject effect = Instantiate(settings.breakEffectPrefab, impactPoint, Quaternion.identity);
            Destroy(effect, settings.effectDuration);
        }
        
        // Notify controller
        glassController.OnGlassBroken(impactPoint, impactForce);
        
        // Invoke event
        OnGlassBreak?.Invoke(impactPoint, impactForce);
        
        Debug.Log($"LookingGlass broken at {impactPoint} with force {impactForce}");
    }
    
    private IEnumerator BreakEffect(Vector3 impactPoint, float impactForce)
    {
        // Switch to broken material
        meshRenderer.material = brokenMaterial;
        
        // Convert impact point to local space
        Vector3 localImpactPoint = transform.InverseTransformPoint(impactPoint);
        brokenMaterial.SetVector(BreakCenter, localImpactPoint);
        
        float elapsedTime = 0f;
        
        while (elapsedTime < settings.effectDuration)
        {
            float progress = elapsedTime / settings.effectDuration;
            float curveValue = settings.breakTransition.Evaluate(progress);
            
            brokenMaterial.SetFloat(BreakProgress, curveValue);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Make glass completely transparent after break effect
        brokenMaterial.SetFloat(BreakProgress, 1f);
        meshRenderer.enabled = false;
    }
    
    private void CreateFractureParticles(Vector3 impactPoint, float impactForce)
    {
        if (settings.fractureParticlePrefab == null) return;
        
        for (int i = 0; i < settings.fractureCount; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * settings.breakRadius;
            Vector3 spawnPosition = impactPoint + randomOffset;
            
            GameObject particle = Instantiate(settings.fractureParticlePrefab, spawnPosition, Random.rotation);
            
            // Add physics force
            Rigidbody particleRb = particle.GetComponent<Rigidbody>();
            if (particleRb != null)
            {
                Vector3 forceDirection = (spawnPosition - impactPoint).normalized;
                if (forceDirection.magnitude < 0.1f)
                    forceDirection = Random.onUnitSphere;
                
                float forceAmount = impactForce * settings.fractureForceMultiplier * Random.Range(0.5f, 1.5f);
                particleRb.AddForce(forceDirection * forceAmount, ForceMode.Impulse);
            }
            
            fractureParticles.Add(particle);
            
            // Schedule destruction
            Destroy(particle, settings.fractureLifetime);
        }
    }
    
    private void PlayBreakSound()
    {
        if (settings.breakSound != null && audioSource != null)
        {
            audioSource.clip = settings.breakSound;
            audioSource.volume = settings.breakVolume;
            audioSource.Play();
        }
    }
    
    public void RepairGlass()
    {
        if (!isBroken || isRepairing || !settings.canRepair) return;
        
        if (repairCoroutine != null)
            StopCoroutine(repairCoroutine);
        repairCoroutine = StartCoroutine(RepairProcess());
    }
    
    private IEnumerator RepairProcess()
    {
        isRepairing = true;
        
        // Play repair sound
        if (settings.repairSound != null && audioSource != null)
        {
            audioSource.clip = settings.repairSound;
            audioSource.volume = settings.breakVolume;
            audioSource.Play();
        }
        
        // Enable mesh renderer
        meshRenderer.enabled = true;
        
        // Animate repair
        float elapsedTime = 0f;
        while (elapsedTime < settings.repairTime)
        {
            float progress = 1f - (elapsedTime / settings.repairTime);
            brokenMaterial.SetFloat(BreakProgress, progress);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Restore original material
        meshRenderer.material = originalMaterial;
        
        // Clean up fracture particles
        foreach (GameObject particle in fractureParticles)
        {
            if (particle != null)
                Destroy(particle);
        }
        fractureParticles.Clear();
        
        // Reset states
        isBroken = false;
        isRepairing = false;
        
        // Re-enable looking glass effect
        glassController.RepairGlass();
        
        // Invoke event
        OnGlassRepair?.Invoke();
        
        Debug.Log("LookingGlass repaired");
    }
    
    // Public methods for external control
    public void SetBreakForce(float force)
    {
        settings.breakForce = force;
    }
    
    public void SetBreakRadius(float radius)
    {
        settings.breakRadius = radius;
        if (brokenMaterial != null)
        {
            brokenMaterial.SetFloat(BreakRadius, radius);
        }
    }
    
    public void ForceBreak(Vector3 impactPoint)
    {
        BreakGlass(impactPoint, settings.breakForce);
    }
    
    public void ForceRepair()
    {
        if (repairCoroutine != null)
            StopCoroutine(repairCoroutine);
        
        isBroken = false;
        isRepairing = false;
        meshRenderer.enabled = true;
        meshRenderer.material = originalMaterial;
        
        // Clean up particles
        foreach (GameObject particle in fractureParticles)
        {
            if (particle != null)
                Destroy(particle);
        }
        fractureParticles.Clear();
        
        glassController.RepairGlass();
        OnGlassRepair?.Invoke();
    }
    
    private void OnDrawGizmos()
    {
        if (!isBroken) return;
        
        // Draw break radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, settings.breakRadius);
        
        // Draw repair radius if repairing
        if (isRepairing && settings.canRepair)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, settings.repairRadius);
        }
    }
    
    private void OnDestroy()
    {
        // Clean up materials
        if (brokenMaterial != null)
        {
            DestroyImmediate(brokenMaterial);
        }
        
        // Clean up particles
        foreach (GameObject particle in fractureParticles)
        {
            if (particle != null)
                DestroyImmediate(particle);
        }
    }
}