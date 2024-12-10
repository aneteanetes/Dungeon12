using Dungeon.ECS;
using Dungeon.View.Interfaces;

namespace Nabunassar.ECS.Components
{
    internal interface ITooltipedDrawText : IComponent
    {
        IDrawText TooltipText { get; }

        bool ShowTooltip { get; }
    }
}
