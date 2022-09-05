using System.Diagnostics;
using Ceres.Engines;
using Ceres.Windowing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Svelto.ECS;
using Svelto.ECS.Internal;
using Svelto.ECS.Schedulers;
using Veldrid;
using Veldrid.Sdl2;

namespace Ceres;

public abstract class CeresApplication
{
    private InitialisationOptions _options;
    private WindowBuilder _windowBuilder;

    private readonly EnginesRoot _ecs  = new(new SimpleEntitiesSubmissionScheduler());
    private IEntityFactory _entityFactory;
    private IServiceProvider _provider;
    private ILogger<CeresApplication>? _log;
    
    private Sdl2Window _window;
    private GraphicsDevice _graphics;
    private CommandList _commands;

    private List<IUpdateEngine> _updateEngines = new();
    private List<IRenderEngine> _renderEngines = new();
    
    internal void Initialise(IServiceProvider provider)
    {
        _provider = provider;
        _options = provider.GetRequiredService<InitialisationOptions>();
        _windowBuilder = provider.GetRequiredService<WindowBuilder>();
        _log = provider.GetService<ILogger<CeresApplication>?>();
        _entityFactory = _ecs.GenerateEntityFactory();
        
        (_window, _graphics, _commands) = _windowBuilder.Build();
        _window.Resized += OnWindowResize;
    }

    protected abstract void Initialise();

    public async Task RunAsync()
    {
        Initialise();
        
        //_ecs.Ready();

        var timer = Stopwatch.StartNew();
        double lastTime = 0;
        
        while (_window.Exists)
        {
            var snapshot = _window.PumpEvents();
            
            _commands.Begin();  
            _commands.SetFramebuffer(_graphics.MainSwapchain.Framebuffer);
            _commands.ClearDepthStencil(1f);

            var delta = timer.Elapsed.TotalSeconds;
            timer.Restart();

            _updateEngines.ForEach(e => e.Update(delta));
            _renderEngines.ForEach(e => e.Render(_commands, _graphics));
            
            _commands.End();
            _graphics.SubmitCommands(_commands);
            _graphics.SwapBuffers(_graphics.MainSwapchain);
            _graphics.WaitForIdle();
        }
        
        _graphics.WaitForIdle();
        _commands.Dispose();
        _graphics.Dispose();
        
        _ecs.Dispose();
    }

    private void OnWindowResize()
    {
        _graphics.MainSwapchain.Resize((uint)_window.Width, (uint)_window.Height);
    }

    protected void AddEngine<T>() where T : class, IEngine
    {
        try
        {
            var engine = ActivatorUtilities.CreateInstance<T>(_provider);

            if (engine is IRenderEngine renderEngine)
                _renderEngines.Add(renderEngine);

            if (engine is IUpdateEngine updateEngine)
                _updateEngines.Add(updateEngine);
            
            _ecs.AddEngine(engine);
        }
        catch (Exception ex)
        {
            _log?.LogWarning(ex, "Could not initialise engine {Engine}", typeof(T).Name);
        }
    }
}