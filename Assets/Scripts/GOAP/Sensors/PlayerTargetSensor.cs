
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace GOAP.Sensors
{
    public class PlayerTargetSensor : LocalTargetSensorBase, IInjectable
    {
    
        private Collider[] Colliders = new Collider[1];
        
        public override void Created() {}

        public override void Update() {}

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget target)
        {
           

            return null;
        }

        public void Inject(DependencyInjector injector){}
        
    }
}