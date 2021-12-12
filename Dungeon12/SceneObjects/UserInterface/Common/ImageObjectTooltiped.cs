using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.SceneObjects.Base;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
