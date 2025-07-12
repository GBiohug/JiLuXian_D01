
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using GOAP.Interfaces;
using GOAP.Tools;
using UnityEngine;

namespace GOAP.Sensors
{
    public class PlayerTargetSensor : LocalTargetSensorBase, IInjectable
    {
        public override void Created() {}

        public override void Update() {}

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget existingTarget)
        {
            Vector3 position = default;
            // Debug.Log("PlayerTargetSensor");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                position = player.transform.position;
            }
            return new PositionTarget(position);
        }
        

        public void Inject(GoapInjector injector){}
        
    }
}