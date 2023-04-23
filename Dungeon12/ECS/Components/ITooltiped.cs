using Dungeon.ECS;

namespace Dungeon12.ECS.Components
{
    internal interface ITooltiped : IComponent
    {
        string TooltipText { get; }
    }
}
