using Dungeon.Drawing.SceneObjects;
using Dungeon.GameObjects;
using System;

namespace Dungeon.SceneObjects.Construct
{
    public class Container : ColoredRectangle<EmptyGameComponent>
    {
        public Container() : base(EmptyGameComponent.Empty)
        {
            Color = ConsoleColor.Black;
            Depth = 1;
            Opacity = 1;
        }
    }
}