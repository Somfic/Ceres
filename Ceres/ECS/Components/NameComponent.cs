using System.Numerics;

namespace Ceres.ECS.Components;

public struct NameComponent
{
    public string Name;
}

public struct TransformComponent
{
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;
}