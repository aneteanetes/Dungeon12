using Dungeon;
using Dungeon.SceneObjects;
using Dungeon.Types;

namespace SidusXII.SceneObjects.Main.Map
{
    public class MapView : EmptySceneControl
    {
        public MapView(Point start)
        {
            Image = "Dungeons/dung.png".AsmImg();

            Width = 750;
            Height = 650;



            ////x15 y 35

        }
    }
}