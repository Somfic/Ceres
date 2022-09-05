using Ceres.Windowing;
using Microsoft.Extensions.DependencyInjection;

namespace Ceres;

public abstract class CeresApplication
{
    private InitialisationOptions _options;
    private WindowBuilder _windowBuilder;

    internal void Initialise(IServiceProvider provider)
    {
        _options = provider.GetRequiredService<InitialisationOptions>();
        _windowBuilder = provider.GetRequiredService<WindowBuilder>();
    }

    public async Task RunAsync()
    {
        var test = _windowBuilder.Build();
        ;
    }
}