using Ceres.Windowing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ceres;

public class CeresApplicationBuilder<T> where T : CeresApplication
{
    private readonly IHostBuilder _builder =  Host.CreateDefaultBuilder();

    internal CeresApplicationBuilder(InitialisationOptions options)
    {
        _builder.ConfigureServices(s =>
        {
            s.AddSingleton(options);
            
            // Windowing
            s.AddTransient<WindowBuilder>();
            
            // User application
            s.AddSingleton<CeresApplication, T>();
        });

        _builder.ConfigureLogging(l =>
        {
            l.ClearProviders();
            l.SetMinimumLevel(LogLevel.Trace);
            l.AddConsole();
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