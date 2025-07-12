using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEngine;
using UnityEngine.AI;
//TODO: decoupling combat and patrol movebehaviour
namespace GOAP.Behaviors
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(AgentBehaviour))]
    public class AgentPatrolMoveBehavior : MonoBehaviour
    {
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private AgentBehaviour agentBehavior;
        private ITarget currentTarget;
        
        [Header("Movement Settings")]
        [SerializeField] private float minMoveDistance = 0.25f;
        [SerializeField] private float minVelocityForMovement = 0.011f;
        [SerializeField] private float rotationSpeed = 10f;
        
        [Header("Combat Settings")]
        [SerializeField] private float combatRotationSpeed = 15f; // 战斗时更快的转向速度
        // [SerializeField] private string playerTag = "Player"; // 玩家标签
        [SerializeField] private float combatFaceDistance = 10f; // 战斗面向距离
        
        private Vector3 lastTargetPosition;
        private bool isInCombat = false;
        private Transform playerTransform;
        
        private static readonly int MOVE_X = Animator.StringToHash("MoveX");
        private static readonly int MOVE_Y = Animator.StringToHash("MoveY");
        private static readonly int ATTACK_TRIGGER = Animator.StringToHash("AttackTrigger");

        public float playerdistance;
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
            agentBehavior.Events.OnTargetInRange += OnTargetInRange;
        }

        private void OnDisable()
        {
            agentBehavior.Events.OnTargetChanged -= OnTargetChanged;
            agentBehavior.Events.OnTargetNotInRange -= OnTargetOutOfRange;
            agentBehavior.Events.OnTargetInRange -= OnTargetInRange;
        }

        private void OnTargetInRange(ITarget target)
        {
            // 检查目标是否是玩家
            if (IsPlayerTarget(target))
            {
                EnterCombat(target);
            }
        }

        private void OnTargetOutOfRange(ITarget target)
        {
            DebugLog("Animator set to idle.");
            SetAnimatorMovement(0f, 0f);
            
            // 如果是玩家离开范围，退出战斗状态
            if (IsPlayerTarget(target))
            {
                ExitCombat();
            }
        }

        private void OnTargetChanged(ITarget target, bool inRange)
        {
            DebugLog($"New target: {target?.Position}, inRange: {inRange}");
            currentTarget = target;
            
            if (currentTarget != null)
            {
                lastTargetPosition = currentTarget.Position;
                navMeshAgent.SetDestination(currentTarget.Position);
                
                // 检查是否是玩家目标
                if (IsPlayerTarget(target) && inRange)
                {
                    EnterCombat(target);
                }
            }
        }
    
        private bool IsPlayerTarget(ITarget target)
        {
            if (target == null) return false;
            
            // 尝试从目标获取GameObject
            GameObject targetObject = null;
            
            if (target is TransformTarget transformTarget)
            {
                targetObject = transformTarget.Transform?.gameObject;
            }
            // 检查是否有玩家标签
            return targetObject != null && targetObject.CompareTag("Player");
        }

        private void EnterCombat(ITarget playerTarget)
        {
            isInCombat = true;
            
            // 获取玩家Transform
            if (playerTarget is TransformTarget transformTarget)
            {
                playerTransform = transformTarget.Transform;
            }

            
            DebugLog("Entered combat mode - will face player");
        }

        private void ExitCombat()
        {
            isInCombat = false;
            playerTransform = null;
            DebugLog("Exited combat mode");
        }

        private void Update()
        {
            if (currentTarget == null) return;

            // 战斗状态特殊处理
            if (isInCombat && playerTransform != null)
            {
                // 更新目标位置
                UpdateDestination();
                
                // 在一定距离内面向玩家
                // float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                // this.playerdistance = distanceToPlayer;
                // if (distanceToPlayer <= combatFaceDistance)
                // {
                //     FaceTarget(playerTransform.position);
                // }
                //
                // 更新动画参数
                UpdateAnimatorParameters();
            }
            else
            {
                // 非战斗状态的正常更新
                UpdateDestination();
                UpdateAnimatorParameters();
            }
        }

        private void FaceTarget(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0; // 保持水平
            
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 
                    combatRotationSpeed * Time.deltaTime);
            }
        }

        private void UpdateDestination()
        {
            // 计算目标点为 agent 和 target 之间的一个点，距离 target 保持一定距离（如0.5米）
            float stopDistance = 3f; // 距离目标的最小距离，可序列化为参数
            Vector3 agentPos = transform.position;
            Vector3 targetPos = currentTarget.Position;
            Vector3 toTarget = targetPos - agentPos;
            float distance = toTarget.magnitude;
            if (distance < Mathf.Epsilon)
                return;

            // 只在目标有明显变化时更新
            float distanceToLastPosition = Vector3.Distance(targetPos, lastTargetPosition);
            if (distanceToLastPosition >= minMoveDistance)
            {
                lastTargetPosition = targetPos;
                // 计算靠近target的点
                Vector3 destination;
                if (distance > stopDistance)
                {
                    destination = targetPos - toTarget.normalized * stopDistance;
                    navMeshAgent.SetDestination(destination);
                }
                else
                {
                    destination = agentPos; // 已经很近了，不移动
                }
                
                DebugLog($"[UpdateDestination] Set destination to {destination}, target at {targetPos}, agent at {agentPos}, stopDistance={stopDistance}");
            }
        }
        
        private void SetDestination(Vector3 position)
        {
                navMeshAgent.SetDestination(position);
            
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
                if (isInCombat && playerTransform != null)
                {
                    // 战斗中优先面向玩家
                    float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                    if (distanceToPlayer <= combatFaceDistance)
                    {
                        FaceTarget(playerTransform.position);
                        return;
                    }
                }
                
                // 正常的移动旋转
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

        // 公共方法，允许外部控制战斗状态
        public void SetCombatMode(bool combat, Transform target = null)
        {
            if (combat && target != null)
            {
                isInCombat = true;
                playerTransform = target;
            }
            else
            {
                ExitCombat();
            }
        }
        
        public bool IsInCombat => isInCombat;
    }
}
