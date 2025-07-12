using CrashKonijn.Goap.Runtime;
using GOAP.Behaviors;
using GOAP.WorldKeys;
using UnityEngine;
using System.Collections;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using UnityEngine;


namespace GOAP.Sensors
{
    public class PlayerDetectionSensor : LocalWorldSensorBase
    {
        private AgentPatrolDetectBehavior _patrolDetectBehavior;
        private AgentPatrolMoveBehavior _patrolMoveBehavior;
        private Transform playerTransform;
        public override void Created() {}

        public override void Update() {}

        public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
        {
             _patrolDetectBehavior = references.GetCachedComponent<AgentPatrolDetectBehavior>();
             GameObject player = GameObject.FindGameObjectWithTag("Player");
             if (player != null)
             {
                 playerTransform = player.transform;
             }
             
            if (_patrolDetectBehavior == null)
                return false;
            bool canSeePlayer = _patrolDetectBehavior.VisibleTargets.Contains(playerTransform);
            if (canSeePlayer)
            {
                _patrolMoveBehavior.SetCombatMode(true,playerTransform);
                Debug.Log($"PlayerDetectionSensor: Player detected at position {playerTransform.position}");
            }
            return canSeePlayer;
        }
    }
}