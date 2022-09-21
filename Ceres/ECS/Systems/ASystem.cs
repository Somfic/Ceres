using DefaultEcs.System;

namespace Ceres.ECS.Systems;

public abstract class ASystem<TState> : ISystem<TState>
{
    public bool IsEnabled { get; set; } = true;

    protected virtual void OnStart(TState state) {}

    private bool _hasInitialized = false;
    public void Update(TState state)
    {
        if (!IsEnabled)
            return;
        
        if (!_hasInitialized)
        {
            OnStart(state);
            _hasInitialized = true;
        }
        
        OnUpdate(state);
    }

    protected abstract void OnUpdate(TState state);

    public virtual void Dispose() {}
}