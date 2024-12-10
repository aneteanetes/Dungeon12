using Dungeon.ECS;

namespace Nabunassar.ECS.Components
{
    internal interface ITooltiped : IComponent
    {
        string TooltipText { get; }
    }
}
