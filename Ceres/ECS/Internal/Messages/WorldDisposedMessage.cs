namespace Ceres.ECS.Internal.Messages
{
    internal readonly struct WorldDisposedMessage
    {
        public readonly int WorldId;

        public WorldDisposedMessage(int worldId)
        {
            WorldId = worldId;
        }
    }
}
