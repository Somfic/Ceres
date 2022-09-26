﻿namespace Ceres.ECS.Internal
{
    internal interface IEntityContainer
    {
        void Add(int entityId);

        void Remove(int entityId);
    }
}
