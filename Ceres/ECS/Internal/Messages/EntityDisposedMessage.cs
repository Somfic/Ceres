namespace Ceres.ECS.Internal.Messages
{
    internal readonly struct EntityDisposedMessage
    {
        public readonly int EntityId;

        public EntityDisposedMessage(int entityId)
        {
            EntityId = entityId;
        }
    }
}
