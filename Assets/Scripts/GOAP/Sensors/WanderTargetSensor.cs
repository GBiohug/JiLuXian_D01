using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using GOAP.Config;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP.Sensors
{
    public class WanderTargetSensor : LocalTargetSensorBase, IInjectable
    {
        private WanderConfigSO WanderConfig;
        public override void Created() {}

        public override void Update() {}

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget existingTarget)
        {
            Vector3 position = GetRandomPosition(agent);
            
            return new PositionTarget(position);
        }

        private Vector3 GetRandomPosition(IActionReceiver agent)
        {
            int count = 0;

            while (count < 5)
            {
                Vector2 random = Random.insideUnitCircle * WanderConfig.WanderRadius;
                Vector3 position = agent.Transform.position + new Vector3(
                    random.x,
                    0,
                    random.y
                );
                if (NavMesh.SamplePosition(position, out NavMeshHit hit, 1, NavMesh.AllAreas))
                {
                    return hit.position;
                }

                count++;
            }

            return agent.Transform.position;
        }

        public void Inject(DependencyInjector injector)
        {
            WanderConfig = injector.WanderConfig;
        }
    }
}