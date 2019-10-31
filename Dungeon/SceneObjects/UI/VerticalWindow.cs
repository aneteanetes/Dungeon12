namespace Dungeon.Drawing.SceneObjects.UI
{
    using Dungeon.Drawing.Impl;
    using System;

    public class VerticalWindow : ImageControl
    {
        public VerticalWindow() : base("Dungeon.Resources.Images.ui.vertical(17x12).png")
        {
            this.Height = 17;
            this.Width = 12;
        }
    }
}