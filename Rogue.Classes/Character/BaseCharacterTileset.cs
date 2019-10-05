using Rogue.Entites.Animations;
using Rogue.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes
{
    public abstract class BaseCharacterTileset : Character
    {
        public override Rectangle TileSetRegion => new Rectangle
        {
            X = 32,
            Y = 0,
            Height = 32,
            Width = 32
        };

        public override AnimationMap MoveUp => new BaseAnimationMap(this.Tileset)
        {
            Direction = Direction.Up,
            Frames = new List<Point>
            {
                //new Point(32,96),
                new Point(64,96),
                new Point(0,96),
                new Point(32,96)
            }
        };

        public override AnimationMap MoveDown => new BaseAnimationMap(this.Tileset)
        {
            Direction = Direction.Down,
            Frames = new List<Point>
            {
                //new Point(32,0),
                new Point(64,0),
                new Point(0,0),
                new Point(32,0)
            }
        };

        public override AnimationMap MoveLeft => new BaseAnimationMap(this.Tileset)
        {
            Direction = Direction.Left,
            Frames = new List<Point>
            {
                //new Point(32,32),
                new Point(64,32),
                new Point(0,32),
                new Point(32,32)
            }
        };

        public override AnimationMap MoveRight => new BaseAnimationMap(this.Tileset)
        {
            Direction = Direction.Right,
            Frames = new List<Point>
            {
                //new Point(32,64),
                new Point(64,64),
                new Point(0,64),
                new Point(32,64)
            }
        };
    }
    

    public class BaseAnimationMap : AnimationMap
    {
        public BaseAnimationMap(string tileSet)
        {
            this.Size = new Point
            {
                X = 32,
                Y = 32
            };

            this.TileSet = tileSet;
        }
    }
}
