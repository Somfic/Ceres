using Microsoft.Extensions.Logging;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace Ceres.Windowing;

public class WindowBuilder
{
    private readonly ILogger<WindowBuilder>? _log;
    private readonly InitialisationOptions _options;

    public WindowBuilder(ILogger<WindowBuilder>? log, InitialisationOptions options)
    {
        _log = log;
        _options = options;
    }
    
    public (Sdl2Window window, GraphicsDevice graphics, CommandList commands) Build()
    {
        _log?.LogTrace("Creating window '{Name}'", _options.Title);
        
        var window = VeldridStartup.CreateWindow(new WindowCreateInfo
        {
            WindowTitle = _options.Title,
            WindowWidth = _options.Size.Width,
            WindowHeight = _options.Size.Height
        });

        window.Resizable = _options.Resizable;
        window.X = (int) _options.Position.X;
        window.Y = (int) _options.Position.Y;
        
#if DEBUG
        const bool isDebug = true;
#else
        const var isDebug = false;
#endif
        
        var graphicsDevice = VeldridStartup.CreateGraphicsDevice(window, new GraphicsDeviceOptions
        {
            Debug = isDebug,
            SyncToVerticalBlank = _options.VSync,
            PreferStandardClipSpaceYDirection = true,
            PreferDepthRangeZeroToOne = true,
            SwapchainDepthFormat = PixelFormat.R16_UNorm,
        }, _options.Backend);
        
        _log?.LogDebug("Created window '{Title}' ({Backend})", _options.Title, graphicsDevice.BackendType.ToString());

        var commandList = graphicsDevice.ResourceFactory.CreateCommandList();
        
        return (window, graphicsDevice, commandList);
    }
}