﻿using Dungeon;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Nabunassar.ECS.Components;
using Nabunassar.SceneObjects.Base;

namespace Nabunassar.SceneObjects.UserInterface.Common
{
    internal class ImageObjectTooltiped : EmptySceneControl, ITooltipedDrawText
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

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
