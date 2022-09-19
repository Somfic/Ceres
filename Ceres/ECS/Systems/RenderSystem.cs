using Ceres.ECS.Components;
using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Extensions.Logging;

namespace Ceres.ECS.Systems;

[With(typeof(NameComponent))]
public class NameSystem : AEntitySetSystem<State>
{
    private readonly ILogger<NameSystem>? _log;

    public NameSystem(World world, IParallelRunner runner, ILogger<NameSystem>? log) : base(world, runner)
    {
        _log = log;
    }

    protected override void Update(State state, in Entity entity)
    {
        ref var name = ref entity.Get<NameComponent>();

        _log?.LogInformation(name.Name);
    }
}