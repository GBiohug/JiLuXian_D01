using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using GOAP.Behaviors;
using GOAP.Config;
using GOAP.Interfaces;
using GOAP.Tools;
using UnityEngine;

namespace GOAP.Actions
{
    public class BattleAction : GoapActionBase<AttackData>, IInjectable
    {   
        //TODO: Implement Battle Config
        private AttackConfigSO attackConfig;
        private Animator animator;
        private GoapActionBase<AttackData> _goapActionBaseImplementation;

        // private bool attacking = false;

        public override void Start(IMonoAgent agent, AttackData data)
        {
            data.Timer = 0f;
            data.Animator.SetBool("ATTACK_TRIGGER", false);
            Debug.Log("attackConfig.MeleeAttackRadius"+ attackConfig.MeleeAttackRadius);
            data.AttackStarted = false;
        }

        public override IActionRunState Perform(IMonoAgent agent, AttackData data, IActionContext context)
        {
            // 攻击冷却处理
            float cooldownTime = attackConfig.MeleeAttackCooldown;
            if (data.Cooldown > 0f)
            {
                data.Cooldown -= context.DeltaTime;
                if (data.Cooldown < 0f) data.Cooldown = 0f;
                return ActionRunState.Continue;
            }

            bool inReach = data.Target != null && Vector3.Distance(data.Target.Position, agent.transform.position) <= attackConfig.MeleeAttackRadius;
            if (inReach && !data.AttackStarted)
            {
                Debug.Log("AttackAnimation Started");
                data.Animator.SetBool("ATTACK_TRIGGER", true);
                data.AttackStarted = true;
            }
            if (inReach)
            {
                agent.transform.LookAt(data.Target.Position);
            }

            // 只要 AttackStarted，检查动画是否播放完毕
            if (data.AttackStarted)
            {
                AnimatorStateInfo stateInfo = data.Animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1.0f)
                {
                    Debug.Log("AttackAnimation Finished");
                    data.Animator.SetBool("ATTACK_TRIGGER", false);
                    data.Cooldown = cooldownTime;
                    data.AttackStarted = false;
                    return ActionRunState.Stop;
                }
                // 如果动画已切换出攻击状态，也可认为结束
                // if (!stateInfo.IsName("Attack") && stateInfo.normalizedTime > 0f)
                // {
                //     data.Cooldown = cooldownTime;
                //     data.AttackStarted = false;
                //     return ActionRunState.Stop;
                // }
                return ActionRunState.Continue;
            }
            // 未进入攻击状态时继续等待
            return ActionRunState.Continue;
        }

        public void Inject(GoapInjector injector)
        {
            this.attackConfig = injector.AttackConfig;
        }
        
        public override void End(IMonoAgent agent, AttackData data)
        {   
            Debug.Log("BattleAction Ended");
            data.Animator.SetBool("ATTACK_TRIGGER", false);
            data.AttackStarted = false;
        }


        
    }
}