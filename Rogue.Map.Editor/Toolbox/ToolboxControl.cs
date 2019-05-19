namespace Rogue.Map.Editor.Toolbox
{
    using Rogue.Drawing;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects;
    using Rogue.Drawing.SceneObjects.Base;
    using System;

    public class ToolboxControl : DarkRectangle
    {
        public override bool AbsolutePosition => true;

        private TextInputControl textInputControl;
        private TileSelector tileSelector;

        public ToolboxControl(Action<ImageControl> select)
        {
            this.Height = 23;
            this.Width = 19;

            textInputControl = new TextInputControl(new DrawText("", new DrawColor(ConsoleColor.White)) { Size = 10 }.Montserrat(), 20)
            {
                OnEnter = s => LoadTileset()
            };
            this.AddChild(textInputControl);
                        
            tileSelector = new TileSelector(select)
            {
                Top = this.textInputControl.Height + 2
            };
            this.AddChild(tileSelector);
        }

        private void LoadTileset()
        {
            var tileset = $"Rogue.Resources.Images.Tiles.{textInputControl.Value}.png";
            tileSelector.Load(tileset);
        }
    }
}
