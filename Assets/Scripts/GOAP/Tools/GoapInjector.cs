using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using GOAP.Config;
using GOAP.Interfaces;
using UnityEngine;

namespace GOAP.Tools
{
    public class GoapInjector : MonoBehaviour, IGoapInjector
    {

        public WanderConfigSO WanderConfig;
        public AttackConfigSO AttackConfig;


        public void Inject(IAction action)
        {
            if (action is IInjectable injectable)
                injectable.Inject(this);
        }

        public void Inject(IGoal goal)
        {
            if (goal is IInjectable injectable)
            {
                injectable.Inject(this);
            }
        }

        public void Inject(ISensor sensor)
        {
            if (sensor is IInjectable injectable)
                injectable.Inject(this);
        }
    }
}