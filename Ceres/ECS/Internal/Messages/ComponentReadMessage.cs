using Ceres.ECS.Serialization;

namespace Ceres.ECS.Internal.Messages
{
    internal readonly struct ComponentReadMessage
    {
        public readonly int EntityId;
        public readonly IComponentReader Reader;

        public ComponentReadMessage(int entityId, IComponentReader reader)
        {
            EntityId = entityId;
            Reader = reader;
        }
    }
}
