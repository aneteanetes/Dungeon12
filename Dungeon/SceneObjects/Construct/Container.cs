using Dungeon.Drawing;
using Dungeon.GameObjects;
using Dungeon.SceneObjects.Base;
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