using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using GOAP.Config;
using GOAP.Interfaces;
using GOAP.Tools;

namespace GOAP.Actions
{
    public class ApproachAction: GoapActionBase<AttackData>, IInjectable
    {   
        private AttackConfigSO AttackConfig;
        public override IActionRunState Perform(IMonoAgent agent, AttackData data, IActionContext context)
        {
            throw new System.NotImplementedException();
        }

        public void Inject(GoapInjector injector)
        {
            AttackConfig = injector.AttackConfig;
        }
    }
}