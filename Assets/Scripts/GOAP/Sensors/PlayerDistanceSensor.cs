
using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using GOAP.Interfaces;
using GOAP.Tools;
using UnityEngine;

namespace GOAP.Sensors
{
    public class PlayerDistanceSensor : LocalWorldSensorBase, IInjectable
    {
        
        
        public override void Created() {}
        public override void Update() {}
        
        public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
        {
            
            return 0;
        }

        public void Inject(GoapInjector injector) {}
    }
}