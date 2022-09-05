using Ceres.Windowing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ceres;

public class CeresApplicationBuilder<T> where T : CeresApplication
{
    private readonly IHostBuilder _builder =  Host.CreateDefaultBuilder();

    internal CeresApplicationBuilder(InitialisationOptions options)
    {
        _builder.ConfigureServices((_, services) =>
        {
            services.AddSingleton(options);
            
            // Windowing
            services.AddTransient<WindowBuilder>();
            
            // User application
            services.AddSingleton<CeresApplication, T>();
        });
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
        
        
        app.Initialise(host.Services);

        return app;
    }
}