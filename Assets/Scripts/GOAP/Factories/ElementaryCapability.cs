
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Runtime;
using GOAP.Goals;
using GOAP.Sensors;
using UnityEngine;

namespace GOAP.Factories
{
    [RequireComponent(typeof(DependencyInjector))]

    public class ElementaryAgentCapability : MonoCapabilityFactoryBase
    {
    private DependencyInjector Injector;

    public override ICapabilityConfig Create()
    {   
        
        Injector = GetComponent<DependencyInjector>();
        CapabilityBuilder builder = new("AISET");

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