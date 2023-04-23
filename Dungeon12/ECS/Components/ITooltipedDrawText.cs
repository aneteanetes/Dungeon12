using Dungeon.ECS;
using Dungeon.View.Interfaces;

namespace Dungeon12.ECS.Components
{
    internal interface ITooltipedDrawText : IComponent
    {
        IDrawText TooltipText { get; }

        bool ShowTooltip { get; }
    }
}
