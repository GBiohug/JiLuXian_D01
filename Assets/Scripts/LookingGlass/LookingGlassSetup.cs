using UnityEngine;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Helper class to set up LookingGlass system quickly
/// </summary>
public static class LookingGlassSetup
{
    public static void SetupLookingGlass(GameObject glassObject, Camera mainCamera = null)
    {
        if (glassObject == null)
        {
            Debug.LogError("LookingGlassSetup: Glass object is null!");
            return;
        }
        
        // Get or create components
        var controller = glassObject.GetComponent<LookingGlassController>();
        if (controller == null)
        {
            controller = glassObject.AddComponent<LookingGlassController>();
        }
        
        var breaker = glassObject.GetComponent<LookingGlassBreaker>();
        if (breaker == null)
        {
            breaker = glassObject.AddComponent<LookingGlassBreaker>();
        }
        
        // Setup main camera reference
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        if (mainCamera != null)
        {
            controller.Settings.targetCamera = mainCamera;
        }
        
        // Setup mesh renderer and material
        var meshRenderer = glassObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("LookingGlassSetup: No MeshRenderer found on glass object!");
            return;
        }
        
        // Load or create LookingGlass material
        var material = Resources.Load<Material>("LookingGlass");
        if (material == null)
        {
            // Create material if it doesn't exist
            var shader = Shader.Find("Universal Render Pipeline/LookingGlass");
            if (shader != null)
            {
                material = new Material(shader);
                material.name = "LookingGlass";
                
                // Set up basic properties
                material.SetFloat("_Transparency", 0.8f);
                material.SetColor("_GlassColor", Color.white);
                material.SetFloat("_RefractionStrength", 0.1f);
                material.SetFloat("_FresnelPower", 1.0f);
                material.SetFloat("_DistortionStrength", 0.05f);
                
                #if UNITY_EDITOR
                // Save material to Resources folder
                string resourcePath = "Assets/Resources";
                if (!AssetDatabase.IsValidFolder(resourcePath))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }
                AssetDatabase.CreateAsset(material, "Assets/Resources/LookingGlass.mat");
                AssetDatabase.SaveAssets();
                #endif
            }
            else
            {
                Debug.LogError("LookingGlassSetup: LookingGlass shader not found!");
                return;
            }
        }
        
        meshRenderer.material = material;
        
        // Setup collider for breaking
        var collider = glassObject.GetComponent<Collider>();
        if (collider == null)
        {
            collider = glassObject.AddComponent<BoxCollider>();
        }
        collider.isTrigger = true;
        
        // Setup render feature
        SetupRenderFeature();
        
        Debug.Log("LookingGlass setup completed on " + glassObject.name);
    }
    
    private static void SetupRenderFeature()
    {
        #if UNITY_EDITOR
        // Find URP asset
        var urpAsset = UniversalRenderPipelineAsset.Create();
        if (urpAsset != null)
        {
            var renderer = urpAsset.scriptableRenderer;
            if (renderer != null)
            {
                // This would need to be done manually in the URP asset
                Debug.Log("Please add LookingGlassRenderFeature to your URP Renderer manually.");
            }
        }
        #endif
    }
    
    public static GameObject CreateLookingGlassPlane(Vector3 position, Vector3 size, Camera mainCamera = null)
    {
        // Create plane mesh
        GameObject glassPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        glassPlane.name = "LookingGlass";
        glassPlane.transform.position = position;
        glassPlane.transform.localScale = size;
        
        // Remove default collider (we'll add our own)
        var defaultCollider = glassPlane.GetComponent<Collider>();
        if (defaultCollider != null)
        {
            Object.DestroyImmediate(defaultCollider);
        }
        
        // Setup LookingGlass components
        SetupLookingGlass(glassPlane, mainCamera);
        
        return glassPlane;
    }
    
    public static void CreateFracturePrefab()
    {
        // Create a simple fracture particle prefab
        GameObject fracturePrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fracturePrefab.name = "GlassFracture";
        
        // Scale it down
        fracturePrefab.transform.localScale = Vector3.one * 0.1f;
        
        // Add rigidbody
        var rb = fracturePrefab.AddComponent<Rigidbody>();
        rb.mass = 0.1f;
        rb.drag = 0.5f;
        rb.angularDrag = 0.5f;
        
        // Add auto-destroy script
        var autoDestroy = fracturePrefab.AddComponent<AutoDestroy>();
        autoDestroy.lifetime = 5f;
        
        // Create transparent material
        var material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.SetFloat("_Surface", 1); // Transparent
        material.SetColor("_BaseColor", new Color(0.8f, 0.8f, 1f, 0.3f));
        
        var renderer = fracturePrefab.GetComponent<MeshRenderer>();
        renderer.material = material;
        
        #if UNITY_EDITOR
        // Save as prefab
        string prefabPath = "Assets/GlassFracture.prefab";
        PrefabUtility.SaveAsPrefabAsset(fracturePrefab, prefabPath);
        Object.DestroyImmediate(fracturePrefab);
        
        Debug.Log("Fracture prefab created at: " + prefabPath);
        #endif
    }
}

/// <summary>
/// Simple auto-destroy component for fracture particles
/// </summary>
public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 5f;
    
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}

#if UNITY_EDITOR
/// <summary>
/// Editor window for easy LookingGlass setup
/// </summary>
public class LookingGlassSetupWindow : EditorWindow
{
    private GameObject targetObject;
    private Camera mainCamera;
    private Vector3 planePosition = Vector3.zero;
    private Vector3 planeSize = Vector3.one;
    
    [MenuItem("Tools/LookingGlass Setup")]
    public static void ShowWindow()
    {
        GetWindow<LookingGlassSetupWindow>("LookingGlass Setup");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("LookingGlass Setup", EditorStyles.boldLabel);
        
        EditorGUILayout.Space();
        
        // Setup existing object
        GUILayout.Label("Setup Existing Object:", EditorStyles.boldLabel);
        targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);
        mainCamera = (Camera)EditorGUILayout.ObjectField("Main Camera", mainCamera, typeof(Camera), true);
        
        if (GUILayout.Button("Setup LookingGlass"))
        {
            if (targetObject != null)
            {
                LookingGlassSetup.SetupLookingGlass(targetObject, mainCamera);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please select a target object!", "OK");
            }
        }
        
        EditorGUILayout.Space();
        
        // Create new plane
        GUILayout.Label("Create New Plane:", EditorStyles.boldLabel);
        planePosition = EditorGUILayout.Vector3Field("Position", planePosition);
        planeSize = EditorGUILayout.Vector3Field("Size", planeSize);
        
        if (GUILayout.Button("Create LookingGlass Plane"))
        {
            var plane = LookingGlassSetup.CreateLookingGlassPlane(planePosition, planeSize, mainCamera);
            Selection.activeGameObject = plane;
        }
        
        EditorGUILayout.Space();
        
        // Utilities
        GUILayout.Label("Utilities:", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Create Fracture Prefab"))
        {
            LookingGlassSetup.CreateFracturePrefab();
        }
        
        if (GUILayout.Button("Add Render Feature Instructions"))
        {
            EditorUtility.DisplayDialog("Render Feature Setup", 
                "To complete the setup:\n\n" +
                "1. Open your URP Renderer asset\n" +
                "2. Add 'LookingGlassRenderFeature' to the Renderer Features list\n" +
                "3. Configure the feature settings as needed\n\n" +
                "The render feature handles depth masking and proper rendering order.", 
                "OK");
        }
    }
}
#endif