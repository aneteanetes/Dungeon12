using Dungeon;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Types;

namespace Dungeon12.SceneObjects.World
{
    public class WorldTileSceneObject : EmptySceneControl
    {
        public override bool Updatable => false;

        private TextObject t;

        public WorldTileSceneObject()
        {
            //Color = DrawColor.DarkBlue;
            Width = WorldSettings.cellSize;
            Height = WorldSettings.cellSize;
            this.Image = "terrain.png";
            
            ImageRegion = new Square()
            {
                Width = 32,
                Height = 32
            };

            t = this.AddTextCenter("F".AsDrawText().InColor(System.ConsoleColor.Red).SegoeUIBold());
            t.Visible=false;
        }

        public override void Focus()
        {
            this.t.Visible=true;
            base.Focus();
        }

        public override void Unfocus()
        {
            this.t.Visible=false;
            base.Unfocus();
        }

        public int x { get; set; }

        public int y { get; set; }

        public void Load(int offsetX, int offsetY)
        {
            if (offsetX == 0 && offsetY == 0)
            {
                offsetX = 12;
                offsetY = 10;
            }

            ImageRegion.X = offsetX * 32;
            ImageRegion.Y = offsetY * 32;
        }
    }
}
