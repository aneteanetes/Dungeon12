namespace Rogue.Map.Editor.Toolbox
{
    using Rogue.Control.Pointer;
    using Rogue.Drawing.SceneObjects;
    using Rogue.Settings;
    using System;

    public class TileSelector : HandleSceneControl
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        private Action<ImageControl> select;

        public TileSelector(Action<ImageControl> select)
        {
            this.select = select;
        }

        public void Load(string tileset)
        {
            this.Image = tileset;

            var measure = Global.DrawClient.MeasureImage(tileset);
            this.Width = measure.X / 32;
            this.Height = measure.Y / 32;
        }

        public override void Click(PointerArgs args)
        {
            var x = args.X/32 - this.Left;
            var y = args.Y/32 - this.Top;
            
            select(new ImageControl(this.Image)
            {
                ImageRegion = new Types.Rectangle
                {
                    Height = 32,
                    Width = 32,
                    X = Math.Truncate(x)*32,
                    Y = Math.Truncate(y)*32
                }
            });
        }
    }
}