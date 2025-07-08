using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;
using UnityEngine;
using CrashKonijn.Goap.Runtime;
using GOAP.Classes;
using GOAP.Goals;

namespace GOAP.Behaviors
{
  
    public class ElementaryBrain : MonoBehaviour
    {

        private GoapActionProvider provider;
        private AgentBehaviour agent;
        private GoapBehaviour goap;
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
            this.provider.RequestGoal<WanderGoal>(true);
        }

        private void OnNoActionFound(IGoalRequest goal)
        {
            this.provider.RequestGoal<WanderGoal>(true);
        }

        private void OnActionEnd(IGoapAction action)
        {
            this.provider.RequestGoal<WanderGoal>(true);
        }
        
        
    }
    
}