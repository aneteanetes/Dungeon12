using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.GameObjects;
using Dungeon.View.Interfaces;
using System;

namespace Dungeon.SceneObjects.Construct
{
    public class Container : ColoredRectangle<GameComponentEmpty>
    {
        public Container() : base(GameComponentEmpty.Empty)
        {
            Color = new DrawColor(ConsoleColor.Black);
            Depth = 1;
            Opacity = 1;
        }
    }
}