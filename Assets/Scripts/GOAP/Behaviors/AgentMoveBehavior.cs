using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP.Behaviors
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(AgentBehaviour))]
    public class AgentMoveBehavior : MonoBehaviour
    {
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private AgentBehaviour agentBehavior;
        private ITarget currentTarget;
        
        [SerializeField] private float minMoveDistance = 0.25f;
        [SerializeField] private float minVelocityForMovement = 0.011f;
        [SerializeField] private float rotationSpeed = 10f; 
        
        private Vector3 lastTargetPosition;
        private static readonly int MOVE_X = Animator.StringToHash("MoveX");
        private static readonly int MOVE_Y = Animator.StringToHash("MoveY");
        private static readonly int ATTACK_TRIGGER = Animator.StringToHash("AttackTrigger");

      
        [SerializeField] private bool enableDebugLogs = false;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            agentBehavior = GetComponent<AgentBehaviour>();
            
       
            navMeshAgent.updatePosition = false; 
            navMeshAgent.updateRotation = false;
            animator.applyRootMotion = true;
        }

        private void OnEnable()
        {
            agentBehavior.Events.OnTargetChanged += OnTargetChanged;
            agentBehavior.Events.OnTargetNotInRange += OnTargetOutOfRange;
        }

        private void OnDisable()
        {
            agentBehavior.Events.OnTargetChanged -= OnTargetChanged;
            agentBehavior.Events.OnTargetNotInRange -= OnTargetOutOfRange;
        }

        private void OnTargetOutOfRange(ITarget target)
        {
            DebugLog("[AgentMoveBehavior] Target out of range. Animator set to idle.");
            SetAnimatorMovement(0f, 0f);
        }

        private void OnTargetChanged(ITarget target, bool inRange)
        {
            DebugLog($"[AgentMoveBehavior] Target changed. New target: {target?.Position}, inRange: {inRange}");
            currentTarget = target;
            
            if (currentTarget != null)
            {
                lastTargetPosition = currentTarget.Position;
                navMeshAgent.SetDestination(currentTarget.Position);
            }
        }

        private void Update()
        {
            if (currentTarget == null) return;

            // 更新目标位置
            UpdateDestination();
            
            // 更新动画参数
            UpdateAnimatorParameters();
        }

        private void UpdateDestination()
        {
            float distanceToLastPosition = Vector3.Distance(currentTarget.Position, lastTargetPosition);
            if (distanceToLastPosition >= minMoveDistance)
            {
                lastTargetPosition = currentTarget.Position;
                navMeshAgent.SetDestination(currentTarget.Position);
            }
        }

        private void UpdateAnimatorParameters()
        {
            Vector3 worldVelocity = navMeshAgent.velocity;
            float currentSpeed = worldVelocity.magnitude;

            if (currentSpeed > minVelocityForMovement)
            {
                Vector3 localVelocity = transform.InverseTransformDirection(worldVelocity);
                SetAnimatorMovement(localVelocity.x, localVelocity.z);
                DebugLog($"[AgentMoveBehavior] Moving: X={localVelocity.x:F2}, Y={localVelocity.z:F2}");
            }
            else
            {
                SetAnimatorMovement(0f, 0f);
                DebugLog("[AgentMoveBehavior] Stopped (speed too low)");
            }
        }

        private void SetAnimatorMovement(float x, float z)
        {
            animator.SetFloat(MOVE_X, x);
            animator.SetFloat(MOVE_Y, z);
        }

        public void TriggerAttackAnimation()
        {
            animator.SetTrigger(ATTACK_TRIGGER);
        }

        private void OnAnimatorMove()
        {
            if (animator.applyRootMotion)
            {
                // 应用 Root Motion 位置
                Vector3 newPosition = transform.position + animator.deltaPosition;
                navMeshAgent.nextPosition = newPosition;
                transform.position = newPosition;

                // 处理旋转
                Vector3 desiredVelocity = navMeshAgent.desiredVelocity;
                if (desiredVelocity.sqrMagnitude > 0.01f)
                {
                    desiredVelocity.y = 0;
                    Quaternion targetRotation = Quaternion.LookRotation(desiredVelocity);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 
                        rotationSpeed * Time.deltaTime);
                }
            }
        }

        private void DebugLog(string message)
        {
            if (enableDebugLogs)
            {
                Debug.Log(message);
            }
        }
    }
}
