using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;

namespace Dungeon12.SceneObjects.Base
{
    internal class TextObjectTooltiped : SceneControl<IDrawText>, ITooltiped
    {
        public TextObjectTooltiped(IDrawText component, string tooltip = "") : base(component)
        {
            Text = component;
            TooltipText = tooltip;
        }

        public string TooltipText { get; set; }

        public void SetText(IDrawText text) => Text = text;
    }
}