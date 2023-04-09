using System;

namespace Dungeon.ECS.Impl
{
    public class ECSComponent : IECSComponent
    {
        public string Name { get; set; }

        public object[] Arguments { get; set; }

        public Type Type { get; set; }

        public ECSComponent(Type type, object[] arguments)
        {
            Type=type;
            Arguments=arguments;
        }
    }
}
