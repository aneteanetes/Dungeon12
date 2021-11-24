using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.ECS.Components
{
    public interface ITooltiped
    {
        IDrawText TooltipText { get; }

        bool ShowTooltip { get; }

        //Tooltip CustomTooltipObject { get; }
    }
}
