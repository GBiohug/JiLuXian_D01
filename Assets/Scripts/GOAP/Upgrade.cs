using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Core
{
    [Obsolete("Use IGoal instead")]
    public interface IGoalBase : IGoal {}
    
    [Obsolete("Use IAction instead")]
    public interface IActionBase : IAction {}

    [Obsolete("Use IGoap instead")]
    public interface IGoapRunner : IGoap {}
    
    [Obsolete("Use IAgentTypeConfig instead")]
    public interface IGoapSetConfig : IAgentTypeConfig {}
    
    [Obsolete("This doesn't exist anymore")]
    public interface IAgentDebugger {}

    public static class UpgradeExtensions
    {
        [Obsolete("Use GetAgentType instead")]
        public static object GetGoapSet(this IGoap goap, string id) => default;
    }
}

namespace CrashKonijn.Goap.Runtime
{
    [Obsolete("Use GoapBehaviour instead")]
    public class GoapRunnerBehaviour : GoapBehaviour, IGoapRunner {}
    
    [Obsolete("Use CapabilityFactoryBase instead")]
    public abstract class GoapSetFactoryBase : CapabilityFactoryBase {}
    
    [Obsolete("Use CapabilityBuilder instead")]
    public class GoapSetBuilder : CapabilityBuilder
    {
        public GoapSetBuilder(string name) : base(name)
        {
        }
    }
    
    public static class UpgradeExtensions
    {
        [Obsolete("This doesn't exist anymore")]
        public static void SetAgentDebugger<T>(this GoapSetBuilder builder) {}
        [Obsolete("This doesn't exist anymore")]
        public static void SetAgentDebugger<T>(this CapabilityBuilder builder) {}
    }
}

namespace CrashKonijn.Agent.Runtime
{
    public static class UpgradeExtensions
    {
        [Obsolete("Use GoapActionProvider.RequestGoal instead")]
        public static void SetGoal<T>(this IAgent agent, bool stopAction = true)
        {
        }
        [Obsolete("Use GoapActionProvider.RequestGoal instead")]
        public static void SetGoal(this IAgent agent, IGoal goal, bool stopAction = true)
        {
        }
    }
}