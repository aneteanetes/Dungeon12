using Dungeon;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.SceneObjects.UserInterface.Common
{
    public class ImageObjectTooltiped : EmptySceneControl, ITooltiped
    {
        public ImageObjectTooltiped(string imagePath, string tooltip)
        {
            TooltipText = tooltip.AsDrawText().Gabriela().InSize(12);
            Image = imagePath;
        }

        public IDrawText TooltipText { get; set; }

        public bool ShowTooltip => true;

        public Tooltip CustomTooltipObject => null;

        public void RefreshTooltip() { }

        public override void Focus()
        {
            base.Focus();
        }
    }
}
