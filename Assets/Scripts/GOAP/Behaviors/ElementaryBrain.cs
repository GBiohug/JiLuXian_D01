using System;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;
using UnityEngine;
using CrashKonijn.Goap.Runtime;
using GOAP.Classes;
using GOAP.Goals;
using GOAP.Sensors;
using GOAP.WorldKeys;

namespace GOAP.Behaviors
{
    //TODO: 完善逻辑+部分测试用动画
    public class ElementaryBrain : MonoBehaviour
    {

        private GoapActionProvider provider;
        private AgentBehaviour agent;
        private GoapBehaviour goap;
        private System.Type currentGoalType;
        
        // [SerializeField] private PlayerSensor PlayerSensor;
        [SerializeField] private AgentPatrolDetectBehavior  patrolDetectBehavior;
        System.Type nextGoalType = null;
        private void Awake()
        {   this.goap = FindObjectOfType<GoapBehaviour>();
            this.agent = this.GetComponent<AgentBehaviour>();
            this.provider = this.GetComponent<GoapActionProvider>();
            if (this.provider.AgentTypeBehaviour == null)
                this.provider.AgentType = this.goap.GetAgentType("ElementaryAgent01");
        }
        
        private void Start()
        {
            this.provider.RequestGoal<WanderGoal>(true);
        }
        private void OnEnable()
        {
            this.provider.Events.OnActionEnd += this.OnActionEnd;
            this.provider.Events.OnNoActionFound += this.OnNoActionFound;
            this.provider.Events.OnGoalCompleted += this.OnGoalCompleted;
        }
        private void OnDisable()
        {
            this.provider.Events.OnActionEnd -= this.OnActionEnd;
            this.provider.Events.OnNoActionFound -= this.OnNoActionFound;
            this.provider.Events.OnGoalCompleted -= this.OnGoalCompleted;
        }
        private void OnGoalCompleted(IGoal goal)
        {
            // this.provider.RequestGoal<WanderGoal>(true);
        }

        private void OnNoActionFound(IGoalRequest goal)
        {
            // this.provider.RequestGoal<WanderGoal>(true);
        }

        private void OnActionEnd(IGoapAction action)
        {   
            Debug.Log($"[ElementaryBrain] Action ended: {action?.GetType().Name}");
            this.DetermineGoal();
            
        }

        public void Update()
        {
            DetermineGoal();
        }

        private void DetermineGoal()
        {

           this.nextGoalType = null;
            if (patrolDetectBehavior.IsDetected)
            {
               
                this.nextGoalType = typeof(KillPlayer);
            }
            else
            {
              
                this.nextGoalType = typeof(WanderGoal);
            }
            
            if (this.currentGoalType != this.nextGoalType)
            {
                this.currentGoalType = this.nextGoalType;
                this.provider.RequestGoal(this.currentGoalType, true);
                Debug.Log($"[ElementaryBrain] Current goal type set to: {this.currentGoalType.Name}");
            }

           
        }
    }
    
}