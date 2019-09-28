namespace Rogue.Classes.Noone
{
    using Rogue.Classes.Noone.Talants;
    using Rogue.Classes.Noone.Talants.Defaura;
    using Rogue.Drawing.Impl;
    using Rogue.Entites.Alive;
    using Rogue.Entites.Animations;
    using Rogue.Types;
    using System;
    using System.Collections.Generic;

    public class Noone : Character
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
        
        public override string ResourceName => "Действия";

        public override string Resource => this.Actions.ToString();

        public override ConsoleColor ResourceColor => ConsoleColor.White;

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

        public int Block { get; set; }

        public int Parry { get; set; }

        public int Stamina { get; set; }

        public int CritChance { get; set; }

        public AbsorbingTalants Absorbing { get; set; } = new AbsorbingTalants();

        public DefensibleTalants Defensible { get; set; } = new DefensibleTalants();

        public override T PropertyOfType<T>()
        {
            switch (typeof(T))
            {
                case Type abs when abs == typeof(AbsorbingTalants): return Absorbing as T;
                case Type abs when abs == typeof(DefensibleTalants): return Defensible as T;
                default: return default;
            }
        }

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

        public override IEnumerable<ClassStat> ClassStats => new ClassStat[]
        {
            new ClassStat("Блок",this.Block.ToString(), new DrawColor(ConsoleColor.White)),
            new ClassStat("Парирование",$"{this.Parry}%", new DrawColor(ConsoleColor.DarkCyan)),
            new ClassStat("Выносливость", $"{this.Stamina}",new DrawColor(ConsoleColor.Yellow)){  Group=1},
            new ClassStat("Шанс крит.", $"{this.CritChance}%",new DrawColor(ConsoleColor.DarkRed)){  Group=1},
        };
    }
}
