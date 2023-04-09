using System;

namespace Dungeon.ECS
{
    public interface IECSComponent
    {
        string Name { get; }

        object[] Arguments { get; }

        public Type Type { get; }
    }
}