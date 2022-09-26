using System.Numerics;
using Ceres.ECS.System;
using Veldrid;

namespace Ceres.Systems;

public class RenderSystem : System<State>
{
    private DeviceBuffer _vertexBuffer;
    private DeviceBuffer _indexBuffer;

    protected override void OnStart(ref State state)
    {
        VertexPositionColor[] quadVertices =
        {
            new(new Vector2(-1f, 1f), RgbaFloat.Red),
            new(new Vector2(1f, 1f), RgbaFloat.Green),
            new(new Vector2(-1f, -1f), RgbaFloat.Blue),
            new(new Vector2(1f, -1f), RgbaFloat.Yellow)
        };
        
        ushort[] quadIndices = { 0, 1, 2, 3 };
        
        _vertexBuffer = state.Graphics.ResourceFactory.CreateBuffer(new BufferDescription(4 * VertexPositionColor.SizeInBytes, BufferUsage.VertexBuffer));
        _indexBuffer = state.Graphics.ResourceFactory.CreateBuffer(new BufferDescription(4 * sizeof(ushort), BufferUsage.IndexBuffer));
        
        state.Graphics.UpdateBuffer(_vertexBuffer, 0, quadVertices);
        state.Graphics.UpdateBuffer(_indexBuffer, 0, quadIndices);
    }

    protected override void OnUpdate(ref State state)
    {
        state.Commands.SetVertexBuffer(0, _vertexBuffer);
        state.Commands.SetIndexBuffer(_indexBuffer, IndexFormat.UInt16);
        state.Commands.DrawIndexed(
            indexCount: 4,
            instanceCount: 1,
            indexStart: 0,
            vertexOffset: 0,
            instanceStart: 0);
    }
}
struct VertexPositionColor
{
    public Vector2 Position; // This is the position, in normalized device coordinates.
    public RgbaFloat Color; // This is the color of the vertex.
    public VertexPositionColor(Vector2 position, RgbaFloat color)
    {
        Position = position;
        Color = color;
    }
    public const uint SizeInBytes = 24;
}