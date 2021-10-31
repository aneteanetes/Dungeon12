using Dungeon.Drawing.SceneObjects;
using Dungeon.GameObjects;
using System;

namespace Dungeon.SceneObjects.Construct
{
    public class Container : ColoredRectangle<GameComponentEmpty>
    {
        public Container() : base(GameComponentEmpty.Empty)
        {
            Color = ConsoleColor.Black;
            Depth = 1;
            Opacity = 1;
        }
    }
}