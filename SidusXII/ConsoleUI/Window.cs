using Dungeon.Drawing;
using Dungeon.SceneObjects;

namespace SidusXII.ConsoleUI
{
    public class Window : EmptySceneObject
    {
        public Border Border { get; set; }

        public DrawText Header { get; set; }

        public int Size { get; set; }

        public string Font { get; set; }

        public override void Initialization()
        {
        }
    }
}