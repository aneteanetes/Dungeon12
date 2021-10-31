using Dungeon.Types;
using Dungeon.View;
using Dungeon12.Entities.Alive;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Classes
{
    public abstract partial class Character : Moveable
    {
        public override Rectangle TileSetRegion => new Rectangle
        {
            X = 32,
            Y = 0,
            Height = 32,
            Width = 32
        };

        public override Animation MoveUp => new BaseAnimationMap(this.Tileset)
        {
            Frames = new List<Point>
            {
                //new Point(32,96),
                new Point(64,96),
                new Point(0,96),
                new Point(32,96)
            }
        };

        public override Animation MoveDown => new BaseAnimationMap(this.Tileset)
        {
            Frames = new List<Point>
            {
                //new Point(32,0),
                new Point(64,0),
                new Point(0,0),
                new Point(32,0)
            }
        };

        public override Animation MoveLeft => new BaseAnimationMap(this.Tileset)
        {
            Frames = new List<Point>
            {
                //new Point(32,32),
                new Point(64,32),
                new Point(0,32),
                new Point(32,32)
            }
        };

        public override Animation MoveRight => new BaseAnimationMap(this.Tileset)
        {
            Frames = new List<Point>
            {
                //new Point(32,64),
                new Point(64,64),
                new Point(0,64),
                new Point(32,64)
            }
        };
    }

    public class BaseAnimationMap : Animation
    {
        public BaseAnimationMap(string tileSet,int x=32, int y=32)
        {
            this.Size = new Point
            {
                X = x,
                Y = y
            };

            this.TileSet = tileSet;
        }
    }
}
