using Ceres.ECS.Serialization;

namespace Ceres.ECS.Internal.Messages
{
    internal readonly struct ComponentTypeReadMessage
    {
        public readonly IComponentTypeReader Reader;

        public ComponentTypeReadMessage(IComponentTypeReader reader)
        {
            Reader = reader;
        }
    }
}
