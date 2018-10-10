namespace Rogue.Classes.BloodMage
{
    using System;
    using System.Collections.Generic;
    using Rogue.Classes.BloodMage.Animations.Moving;
    using Rogue.Entites.Alive.Character;
    using Rogue.Entites.Animations;
    using Rogue.Types;

    public class BloodMage : Player
    {
        public BloodMage()
        {
            this.HitPoints = this.MaxHitPoints = 100;
            this.Level = 1;
            this.Blood = 100;
            this.MinDMG = 1;
            this.MaxDMG = 2;
        }

        public override string ClassName { get => "Маг крови"; }

        public override string Resource => this.Blood.ToString();

        public override string ResourceName => "Кровь";

        public override ConsoleColor ResourceColor => ConsoleColor.Red;

        public override ConsoleColor ClassColor => ConsoleColor.Red;

        public long Blood { get; set; }

        public override string Tileset => "Rogue.Classes.BloodMage.Images.Dolls.Character.png";

        public override Rectangle TileSetRegion => new Rectangle
        {
            X = 32,
            Y = 0,
            Height = 32,
            Width = 32
        };

        public override AnimationMap MoveUp => new BaseMove
        {
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
            Frames = new List<Point>
            {
                //new Point(32,64),
                new Point(64,64),
                new Point(0,64),
                new Point(32,64)
            }
        };
    }
}