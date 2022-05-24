using Dungeon.View.Interfaces;

namespace Dungeon12.ECS.Components
{
    internal interface ITooltipedDrawText
    {
        IDrawText TooltipText { get; }

        bool ShowTooltip { get; }
    }
}
