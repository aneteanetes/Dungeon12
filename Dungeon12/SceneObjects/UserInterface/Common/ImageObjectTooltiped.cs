using Dungeon;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.SceneObjects.UserInterface.Common
{
    internal class ImageObjectTooltiped : EmptySceneControl, ITooltipedDrawText
    {
        public ImageObjectTooltiped(string imagePath, string tooltip)
        {
            TooltipText = tooltip.AsDrawText().Gabriela().InSize(12);
            Image = imagePath;
        }

        public override bool PerPixelCollision => true;

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
