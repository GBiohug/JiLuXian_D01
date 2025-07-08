
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Runtime;
using GOAP.Actions;
using GOAP.Goals;
using GOAP.Sensors;
using GOAP.Tools;
using UnityEngine;

namespace GOAP.Factories
{
    [RequireComponent(typeof(GoapInjector))]

    public class ElementaryCapability : CapabilityFactoryBase
    {
    // private GoapInjector Injector;

    public override ICapabilityConfig Create()
    {   
        
        // Injector = GetComponent<DependencyInjector>();
        CapabilityBuilder builder = new("ElementaryCapability");

        BuildGoals(builder);
        BuildActions(builder);
        BuildSensors(builder);

        return builder.Build();
    }

    private void BuildGoals(CapabilityBuilder builder)
    {
        builder.AddGoal<WanderGoal>()
            .AddCondition<IsWandering>(Comparison.GreaterThanOrEqual, 1);

        builder.AddGoal<KillPlayer>()
            .AddCondition<PlayerHealth>(Comparison.SmallerThanOrEqual, 0);
        
    }

    private void BuildActions(CapabilityBuilder builder)
    {
        builder.AddAction<WanderAction>()
            .SetTarget<WanderTarget>()
            .AddEffect<IsWandering>(EffectType.Increase)
            .SetBaseCost(5)
            .SetInRange(10);
    }

    private void BuildSensors(CapabilityBuilder builder)
    {
        builder.AddTargetSensor<WanderTargetSensor>()
            .SetTarget<WanderTarget>();

        builder.AddTargetSensor<PlayerTargetSensor>()
            .SetTarget<PlayerTarget>();
        
        builder.AddWorldSensor<PlayerDistanceSensor>()
            .SetKey<PlayerDistance>();
        
    }
    }
}