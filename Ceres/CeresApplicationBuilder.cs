using Ceres.Assets;
using Ceres.ECS;
using Ceres.ECS.System;
using Ceres.ECS.Threading;
using Ceres.Windowing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ceres;

public class CeresApplicationBuilder<T> where T : CeresApplication
{
    private readonly IHostBuilder _builder =  Host.CreateDefaultBuilder();
    private readonly List<Type> _systems = new();
    private readonly List<Type> _resources = new();
    private readonly World _world = new();
    private readonly IParallelRunner _parallelRunner = new DefaultParallelRunner(Environment.ProcessorCount);

    internal CeresApplicationBuilder(InitialisationOptions options)
    {
        _builder.ConfigureServices((_, services) =>
        {
            services.AddSingleton(options);
            
            // Windowing
            services.AddTransient<WindowBuilder>();
            
            // User application
            services.AddSingleton<CeresApplication, T>();
            
            // Asset loader
            services.AddTransient<AssetLoader>();
            
            // ECS
            services.AddSingleton(_world);
            services.AddSingleton(_parallelRunner);
        });
    }

    public CeresApplicationBuilder<T> AddTransient<TService>() where TService : class
    {
        _builder.ConfigureServices(s => s.AddTransient<TService>());
        return this;
    }
    
    public CeresApplicationBuilder<T> AddTransient<TService, TImplementation>() where TService : class where TImplementation : class, TService
    {
        _builder.ConfigureServices(s => s.AddTransient<TService, TImplementation>());
        return this;
    }
    
    public CeresApplicationBuilder<T> AddSingleton<TService>() where TService : class
    {
        _builder.ConfigureServices(s => s.AddSingleton<TService>());
        return this;
    }
    
    public CeresApplicationBuilder<T> AddSingleton<TService, TImplementation>() where TService : class where TImplementation : class, TService
    {
        _builder.ConfigureServices(s => s.AddSingleton<TService, TImplementation>());
        return this;
    }
    
    public CeresApplicationBuilder<T> AddSystem<TSystem>() where TSystem : class, ISystem<State>
    {
        _systems.Add(typeof(TSystem));
        return this;
    }

    public async Task RunAsync()
    {
        var app = Build();
        await app.RunAsync();
    }

    private T Build()
    {
        var host = _builder.Build();
        var app = (T)host.Services.GetRequiredService<CeresApplication>();

        if (app == null)
            throw new Exception("Could not build application");
        
        app.Initialise(host.Services, _systems, _resources);

        return app;
    }

    public CeresApplicationBuilder<T> AddResource<TResourceManager>()
    {
        _resources.Add(typeof(TResourceManager));
        return this;
    }
}