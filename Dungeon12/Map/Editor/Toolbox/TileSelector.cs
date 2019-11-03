namespace Dungeon12.Map.Editor.Toolbox
{
    using Dungeon;
    using Dungeon.Control.Pointer;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.SceneObjects;
    using System;

    public class TileSelector : HandleSceneControl
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        private Action<ImageControl> select;

        public bool FullTile { get; set; }

        public TileSelector(Action<ImageControl> select)
        {
            this.select = select;
        }

        public void Load(string tileset)
        {
            try
            {
                var measure = Global.DrawClient.MeasureImage(tileset);
                this.Width = measure.X / 32;
                this.Height = measure.Y / 32;

                this.Image = tileset;
            }
            catch { }
        }

        public override void Click(PointerArgs args)
        {
            if (FullTile)
            {
                select(new ImageControl(this.Image));
                return;
            }

            var x = args.X/32 - this.Left;
            var y = args.Y/32 - this.Top;
            
            select(new ImageControl(this.Image)
            {
                ImageRegion = new Dungeon.Types.Rectangle
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