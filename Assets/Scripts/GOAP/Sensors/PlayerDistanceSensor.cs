using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using GOAP.Config;
using GOAP.Interfaces;
using GOAP.Tools;
using UnityEngine;

namespace GOAP.Sensors
{
    public class PlayerDistanceSensor : LocalWorldSensorBase, IInjectable
    {
        
        private AttackConfigSO AttackConfig;
        public override void Created() {}
        public override void Update() {}
        
        public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
        {
            float distance = 0f;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                distance = Vector3.Distance(agent.Transform.position, player.transform.position);
            }

            if (distance<AttackConfig.MeleeAttackRadius)
            {
                return 1;
            }
            return 0;
        }

        public void Inject(GoapInjector injector)
        {
            AttackConfig = injector.AttackConfig;
        }
    }
}