using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using GOAP.Config;

namespace GOAP
{
    public class DependencyInjector : GoapConfigInitializerBase, IGoapInjector
    {
     
        public WanderConfigSO WanderConfig;
        
        public override void InitConfig(IGoapConfig config)
        {
            config.GoapInjector = this;
        }

        public void Inject(IAction action)
        {
            if (action is IInjectable injectable)
            {
                injectable.Inject(this);
            }
        }

        public void Inject(IGoal goal)
        {
            if (goal is IInjectable injectable)
            {
                injectable.Inject(this);
            }
        }

        public void Inject(ISensor worldSensor)
        {
            if (worldSensor is IInjectable injectable)
            {
                injectable.Inject(this);
            }
        }

        public void Inject(ITargetSensor targetSensor)
        {
            if (targetSensor is IInjectable injectable)
            {
                injectable.Inject(this);
            }
        }
    }
}