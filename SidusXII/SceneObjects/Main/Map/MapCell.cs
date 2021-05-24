using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;

namespace SidusXII.SceneObjects.Main.Map
{
    public class MapCell : BlackRectangle
    {
        int x, y;

        public MapCell(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.Image = "GUI/Parts/cellback.png".AsmImg();
            Depth = 1;
        }

        public override void Click(PointerArgs args)
        {
            System.Console.WriteLine($"x:{x}, y:{y}");
            base.Click(args);
        }
    }
}