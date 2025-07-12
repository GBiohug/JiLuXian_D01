using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace GOAP.Behaviors
{
[RequireComponent(typeof(SphereCollider))]
public class AgentPatrolDetectBehavior : MonoBehaviour
{
    [Header("Vision Settings")]
    [SerializeField] private float viewRadius = 10f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float heightOffset = 1.5f; // 眼睛高度
    
    [Header("Detection Settings")]
    [SerializeField] private float detectionUpdateInterval = 0.2f;
    [SerializeField] private float suspicionIncreaseRate = 1f;
    [SerializeField] private float suspicionDecreaseRate = 0.5f;
    [SerializeField] private float maxSuspicion = 100f;
    
    [SerializeField] private AgentPatrolMoveBehavior moveBehavior;
    public bool IsDetected = false;
    // 检测到的目标
    private Dictionary<Transform, float> detectedTargets = new Dictionary<Transform, float>();
    private List<Transform> targetsInView = new List<Transform>();
    
    // 事件
    public Action<Transform> OnTargetDetected;
    public Action<Transform> OnTargetLost;
    public Action<Transform, float> OnSuspicionChanged;
    
    private SphereCollider detectionCollider;
    private float lastDetectionTime;
    
    public List<Transform> VisibleTargets => targetsInView;
    public bool HasDetectedPlayer => targetsInView.Count > 0;
    
    private void Awake()
    {
        // 设置触发器碰撞体
        detectionCollider = GetComponent<SphereCollider>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = viewRadius;
    }
    
    private void Update()
    {
        if (Time.time - lastDetectionTime >= detectionUpdateInterval)
        {
            lastDetectionTime = Time.time;
            UpdateVisibleTargets();
            UpdateSuspicionLevels();
        }
    }
    
    private void UpdateVisibleTargets()
    {
        targetsInView.Clear();
        
        // 获取视野范围内的所有目标
        Collider[] targetsInViewRadius = Physics.OverlapSphere(
            transform.position + Vector3.up * heightOffset, 
            viewRadius, 
            targetMask
        );
        
        foreach (Collider target in targetsInViewRadius)
        {
            Transform targetTransform = target.transform;
            Vector3 dirToTarget = (targetTransform.position - (transform.position + Vector3.up * heightOffset)).normalized;
            
            // 检查是否在视角范围内
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(
                    transform.position + Vector3.up * heightOffset, 
                    targetTransform.position
                );
                
                // 射线检测是否有障碍物
                if (!Physics.Raycast(
                    transform.position + Vector3.up * heightOffset, 
                    dirToTarget, 
                    distToTarget, 
                    obstacleMask))
                {
                    targetsInView.Add(targetTransform);
                    
                    // 新检测到的目标
                    if (!detectedTargets.ContainsKey(targetTransform))
                    {
                        detectedTargets[targetTransform] = 0f;
                        OnTargetDetected?.Invoke(targetTransform);
                        IsDetected = true;
                       
                        Debug.Log($"[AgentPatrolBehavior] Detected target: {targetTransform.name}");
                    }
                }
            }
        }
        
        // 检查丢失的目标
        List<Transform> lostTargets = new List<Transform>();
        foreach (var kvp in detectedTargets)
        {
            if (!targetsInView.Contains(kvp.Key))
            {
                lostTargets.Add(kvp.Key);
            }
        }
        
        // 处理丢失的目标
        foreach (var target in lostTargets)
        {
            if (detectedTargets[target] <= 0)
            {
                detectedTargets.Remove(target);
                OnTargetLost?.Invoke(target);
                IsDetected = false;
            }
        }
    }
    
    private void UpdateSuspicionLevels()
    {
        List<Transform> toRemove = new List<Transform>();
        
        foreach (var kvp in detectedTargets.ToList())
        {
            Transform target = kvp.Key;
            float currentSuspicion = kvp.Value;
            
            if (targetsInView.Contains(target))
            {
                // 增加怀疑度
                float newSuspicion = Mathf.Min(
                    currentSuspicion + suspicionIncreaseRate * detectionUpdateInterval, 
                    maxSuspicion
                );
                detectedTargets[target] = newSuspicion;
                OnSuspicionChanged?.Invoke(target, newSuspicion / maxSuspicion);
            }
            else
            {
                // 减少怀疑度
                float newSuspicion = Mathf.Max(
                    currentSuspicion - suspicionDecreaseRate * detectionUpdateInterval, 
                    0f
                );
                
                if (newSuspicion <= 0)
                {
                    toRemove.Add(target);
                }
                else
                {
                    detectedTargets[target] = newSuspicion;
                    OnSuspicionChanged?.Invoke(target, newSuspicion / maxSuspicion);
                }
            }
        }
        
        foreach (var target in toRemove)
        {
            detectedTargets.Remove(target);
        }
    }
    
    public float GetSuspicionLevel(Transform target)
    {
        return detectedTargets.ContainsKey(target) 
            ? detectedTargets[target] / maxSuspicion 
            : 0f;
    }
    
    public bool IsTargetFullyDetected(Transform target)
    {
        return detectedTargets.ContainsKey(target) && 
               detectedTargets[target] >= maxSuspicion;
    }
    
    // 可视化调试
    private void OnDrawGizmosSelected()
    {
        // 绘制视野范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * heightOffset, viewRadius);
        
        // 绘制视角
        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);
        
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position + Vector3.up * heightOffset, 
            transform.position + Vector3.up * heightOffset + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position + Vector3.up * heightOffset, 
            transform.position + Vector3.up * heightOffset + viewAngleB * viewRadius);
        
        // 绘制检测到的目标
        Gizmos.color = Color.red;
        foreach (Transform target in targetsInView)
        {
            if (target != null)
            {
                Gizmos.DrawLine(transform.position + Vector3.up * heightOffset, target.position);
                
                float suspicion = GetSuspicionLevel(target);
                Gizmos.color = Color.Lerp(Color.yellow, Color.red, suspicion);
                Gizmos.DrawWireSphere(target.position, 0.5f);
            }
        }
    }
    
    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
}