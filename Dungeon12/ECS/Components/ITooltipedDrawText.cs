using Dungeon.View.Interfaces;

namespace Dungeon12.ECS.Components
{
    public interface ITooltipedDrawText
    {
        IDrawText TooltipText { get; }

        bool ShowTooltip { get; }
    }
}
