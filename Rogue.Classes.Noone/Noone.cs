namespace Rogue.Classes.Noone
{
    using Rogue.Entites.Alive.Character;
    using Rogue.Entites.Animations;
    using Rogue.Types;
    using System.Collections.Generic;

    public class Noone : Player
    {
        public Noone()
        {
            var timer = new System.Timers.Timer(3000);
            timer.Elapsed += RestoreActions;
            timer.Start();

            this.HitPoints = this.MaxHitPoints = 100;
            this.Level = 1;
            this.MinDMG = 1;
            this.MaxDMG = 2;

            this.Actions = 5;
        }

        public override string Avatar => "Rogue.Classes.Noone.Images.noone.png";

        public override string ClassName { get => "Приключенец"; }
        
        public override string ResourceName => "Дейсвия";

        private void RestoreActions(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Actions >= 5)
            {
                return;
            }

            Actions++;
        }

        public int Actions { get; set; } = 5;

        public override string Tileset => "Rogue.Classes.Noone.Images.sprite.png";

        public override Rectangle TileSetRegion => new Rectangle
        {
            X = 32,
            Y = 0,
            Height = 32,
            Width = 32
        };

        public override AnimationMap MoveUp => new BaseMove
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

        public override AnimationMap MoveDown => new BaseMove
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

        public override AnimationMap MoveLeft => new BaseMove
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

        public override AnimationMap MoveRight => new BaseMove
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

        private class BaseMove : AnimationMap
        {
            public BaseMove()
            {
                this.Size = new Point
                {
                    X = 32,
                    Y = 32
                };

                this.TileSet = "Rogue.Classes.Noone.Images.sprite.png";
            }
        }
    }
}
