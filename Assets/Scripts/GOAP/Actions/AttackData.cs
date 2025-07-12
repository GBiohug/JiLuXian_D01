using CrashKonijn.Agent.Runtime;
using GOAP.Behaviors;
using UnityEngine;

namespace GOAP.Actions
{
    public class AttackData : CommonData
    {   
        public bool AttackStarted = false;
        [GetComponent]
        public AgentPatrolMoveBehavior agentPatrolMoveBehavior{ get; set; }
        [GetComponent]
        public Animator Animator { get; set; }

        public float Cooldown = 5f;

        public bool HasCooldown = false;
    }
}