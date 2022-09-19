using DefaultEcs.System;

namespace Ceres.ECS.Systems;

public abstract class ASystem<TState> : ISystem<TState>
{
    public bool IsEnabled { get; set; } = true;

    public abstract void Update(TState state);

    public void Dispose() {}
}