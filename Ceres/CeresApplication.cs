using System.Diagnostics;
using Assimp.Unmanaged;
using Ceres.Components;
using Ceres.ECS;
using Ceres.ECS.Resource;
using Ceres.ECS.System;
using Ceres.Systems;
using Ceres.Windowing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Veldrid;
using Veldrid.Sdl2;

namespace Ceres;

public abstract class CeresApplication
{
    private InitialisationOptions _options;
    private WindowBuilder _windowBuilder;
    private List<System<State>> _systems = new();
    private ILogger<CeresApplication>? _log;
    private World _world;
    private Sdl2Window _window;
    private GraphicsDevice _graphics;
    private CommandList _commands;

    internal void Initialise(IServiceProvider provider, List<Type> systems, List<Type> resources)
    {
        _log = provider.GetService<ILogger<CeresApplication>>();
        _world = provider.GetRequiredService<World>();
        _options = provider.GetRequiredService<InitialisationOptions>();
        _windowBuilder = provider.GetRequiredService<WindowBuilder>();
        
        (_window, _graphics, _commands) = _windowBuilder.Build();
        _window.Resized += () => _graphics.ResizeMainWindow((uint) _window.Width, (uint) _window.Height);
        
        foreach (var systemType in systems)
        {
            if (ActivatorUtilities.CreateInstance(provider, systemType) is not System<State> system)
            {
                _log?.LogWarning("Could not create system {SystemType}", systemType.Name);
                continue;
            }
            
            _systems.Add(system);
        }

        foreach (var resourceType in resources)
        {
            if (ActivatorUtilities.CreateInstance(provider, resourceType, _graphics) is not IResourceManager resource)
            {
                _log?.LogWarning("Could not create system {SystemType}", resourceType.Name);
                continue;
            }

            resource.Manage(_world);
        }

        _world.SetMaxCapacity<NameComponent>(1);
    }

    public async Task RunAsync()
    {
        try
        {
            Start();

            var state = new State
            {
                Window = _window,
                Graphics = _graphics,
                Commands = _commands,
                Gui = new GuiState()
            };

            while (_window.Exists)
            {
                state.Input = _window.PumpEvents();
                state.Commands.Begin();

                foreach (var system in _systems)
                {
                    try
                    {
                        system.UpdateRef(ref state);
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
        catch (Exception ex)
        {
            _log?.LogCritical(ex, "Error in application");
        }
    }

    protected Entity CreateEntity(string name)
    {
        var entity = _world.CreateEntity();
        entity.Set(new NameComponent { Name = name });
        return entity;
    }

    protected abstract void Start();
}