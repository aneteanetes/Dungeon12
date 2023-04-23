using System;
using System.ComponentModel;

namespace Dungeon.ECS
{
    public interface IECSComponent
    {
        string Name { get; }

        object[] Arguments { get; }

        public Type Type { get; }
    }
}