using System.Diagnostics;
using Ceres.ECS;
using Ceres.ECS.Components;
using Ceres.Windowing;
using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Veldrid;

namespace Ceres;

public abstract class CeresApplication
{
    private InitialisationOptions _options;
    private WindowBuilder _windowBuilder;
    private List<ISystem<State>> _systems = new();
    private ILogger<CeresApplication>? _log;
    private World _world;

    internal void Initialise(IServiceProvider provider, List<Type> systems)
    {
        _log = provider.GetService<ILogger<CeresApplication>>();
        _world = provider.GetRequiredService<World>();
        _options = provider.GetRequiredService<InitialisationOptions>();
        _windowBuilder = provider.GetRequiredService<WindowBuilder>();
        
        foreach (var systemType in systems)
        {
            var system = ActivatorUtilities.CreateInstance(provider, systemType) as ISystem<State>;

            if (system == null)
            {
                _log?.LogWarning("Could not create system {SystemType}", systemType.Name);
                continue;
            }
            
            _systems.Add(system);
        }

        _world.SetMaxCapacity<NameComponent>(1);
    }

    public async Task RunAsync()
    {
        var (window, graphics, commands) = _windowBuilder.Build();
        Start();

        var state = new State
        {
            Window = window, 
            Graphics =  graphics, 
            Commands = commands
        };
        
        while (window.Exists)
        {
            state.Input = window.PumpEvents();
            
            state.Commands.Begin();
            state.Commands.SetFramebuffer(graphics.SwapchainFramebuffer);
            state.Commands.ClearColorTarget(0, RgbaFloat.Red);

            foreach (var system in _systems)
            {
                try
                {
                    system.Update(state);
                }
                catch (Exception ex)
                {
                    _log?.LogError(ex, "Error in system {SystemType}", system.GetType().Name);
                    system.IsEnabled = false;
                }
            }
            
            state.Commands.End();
            
            state.Graphics.SubmitCommands(state.Commands);
            state.Graphics.SwapBuffers();
            state.Graphics.WaitForIdle();
        }
        
        _world.Dispose();
    }

    protected Entity CreateEntity(string name)
    {
        var entity = _world.CreateEntity();
        entity.Set(new NameComponent { Name = name });
        return entity;
    }

    protected abstract void Start();
}