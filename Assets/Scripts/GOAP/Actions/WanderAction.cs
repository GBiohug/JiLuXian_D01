using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using GOAP.Config;
using GOAP.Interfaces;
using GOAP.Tools;
using UnityEngine;

namespace GOAP.Actions
{
    public class WanderAction : GoapActionBase<CommonData>, IInjectable
    {
        private WanderConfigSO WanderConfig;
        
        public override void Created() {}

        public override void Start(IMonoAgent agent, CommonData data)
        {
            // data.Timer = Random.Range(WanderConfig.WaitRangeBetweenWanders.x, WanderConfig.WaitRangeBetweenWanders.y);
            data.Timer = 10; // For testing purposes, set a fixed timer
        }



        public override IActionRunState Perform(IMonoAgent agent, CommonData data, IActionContext context)
        {
            data.Timer -= context.DeltaTime;

            if (data.Timer > 0)
            {
                return ActionRunState.Continue;
            }

            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, CommonData data) {}
        
        public void Inject(GoapInjector injector)
        {
            WanderConfig = injector.WanderConfig;
        }
}

}