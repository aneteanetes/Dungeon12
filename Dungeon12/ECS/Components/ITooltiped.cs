using Dungeon.View.Interfaces;

namespace Dungeon12.ECS.Components
{
    public interface ITooltiped
    {
        IDrawText TooltipText { get; }

        bool ShowTooltip { get; }

        void RefreshTooltip();
    }
}
