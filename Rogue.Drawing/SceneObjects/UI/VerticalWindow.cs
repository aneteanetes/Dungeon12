namespace Rogue.Drawing.SceneObjects.UI
{
    using Rogue.Drawing.Impl;
    using System;

    public class VerticalWindow : ImageControl
    {
        public VerticalWindow() : base("Rogue.Resources.Images.ui.vertical(17x12).png")
        {
            this.Height = 17;
            this.Width = 12;
        }
    }
}