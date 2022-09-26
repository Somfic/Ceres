namespace Ceres.ECS.System;

public abstract class System<TState> : ISystem<TState>
{
    protected TState State { get; private set; }
    
    public void Update(TState state)
    {
        State = state;
    }

    public bool IsEnabled { get; set; } = true;

    protected virtual void OnStart(ref TState state) {}

    private bool _hasInitialized = false;
    internal void UpdateRef(ref TState state)
    {
        Update(state);
        
        if (!IsEnabled)
            return;
        
        if (!_hasInitialized)
        {
            OnStart(ref state);
            _hasInitialized = true;
            return;
        }
        
        OnUpdate(ref state);
    }

    protected abstract void OnUpdate(ref TState state);

    public virtual void Dispose() {}
}