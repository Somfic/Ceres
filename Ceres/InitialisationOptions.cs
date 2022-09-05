using System.Drawing;
using System.Numerics;
using Veldrid;

namespace Ceres;

public class InitialisationOptions
{
    public InitialisationOptions()
    {
    }

    /// <summary>The name of the window.</summary>
    public string Title { get; init; } = "Ceres";

    /// <summary> The size of the window, in pixels.</summary>
    public Size Size { get; init; } = new(500, 500);

    /// <summary>The position of the window, in pixels.</summary>
    public Vector2 Position { get; init; } = new(500);

    /// <summary>Whether VSync is enabled for the window.</summary>
    public bool VSync { get; init; } = false;

    /// <summary>Whether to started the window centered on the monitor.</summary>
    public bool StartCentered { get; init; } = true;

    /// <summary>Whether the window is resizeable.</summary>
    public bool Resizable { get; init; } = false;

    /// <summary>The sample size used when refreshing the window.</summary>
    public int SampleSize { get; init; } = 4;

    /// <summary>Whether to keep the window top-most.</summary>
    public bool TopMost { get; init; } = false;
    
    /// <summary>Whether to focus the window upon load.</summary>
    public bool FocusOnLoad { get; init; } = true;

    /// <summary>The graphics API to use.</summary>
    public GraphicsBackend Backend { get; init; } = GraphicsBackend.OpenGL;
}