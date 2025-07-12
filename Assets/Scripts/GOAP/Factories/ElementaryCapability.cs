
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Runtime;
using GOAP.Actions;
using GOAP.Config;
using GOAP.Goals;
using GOAP.Interfaces;
using GOAP.Sensors;
using GOAP.Tools;
using GOAP.WorldKeys;
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

        builder.AddGoal<ApproachGoal>()
            .AddCondition<PlayerWithinReach>(Comparison.GreaterThanOrEqual, 1);


    }

    private void BuildActions(CapabilityBuilder builder)
    {
        builder.AddAction<WanderAction>()
            .SetTarget<WanderTarget>()
            .AddEffect<IsWandering>(EffectType.Increase)
            .SetBaseCost(5)
            .SetInRange(10);

        builder.AddAction<BattleAction>()
            .SetTarget<PlayerTarget>()
            .AddEffect<PlayerHealth>(EffectType.Decrease)
            .SetBaseCost(5)
            .SetStoppingDistance(30);
        
        builder.AddAction<ApproachAction>()
            .SetTarget<PlayerTarget>()
            .AddEffect<PlayerWithinReach>(EffectType.Increase)
            .SetBaseCost(5)
            .SetInRange(5);
    }

    private void BuildSensors(CapabilityBuilder builder)
    {
        builder.AddTargetSensor<WanderTargetSensor>()
            .SetTarget<WanderTarget>();

        builder.AddTargetSensor<PlayerTargetSensor>()
            .SetTarget<PlayerTarget>();

        builder.AddWorldSensor<PlayerDetectionSensor>()
            .SetKey<PlayerFullyDetected>();

        builder.AddWorldSensor<PlayerDistanceSensor>()
            .SetKey<PlayerWithinReach>();

    }
    
    }
}