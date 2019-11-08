namespace Dungeon.Map.Objects
{
    using System;
    using System.Collections.Generic;
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Entities.Alive;
    using Dungeon.Entities.Enemy;
    using Dungeon.Game;
    using Dungeon.Map.Infrastructure;
    using Dungeon.Physics;
    using Dungeon.Settings;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;

    [Template("*")]
    public class Mob : EntityMapObject<Enemy>
    {
        public Mob(Enemy component):base(component)
        {

        }

        public override string Icon { get => "*"; set { } }

        protected override MapObject Self => this;
        
        public override bool Obstruction => true;

        public bool IsChasing { get; set; }

        public override double MovementSpeed => base.MovementSpeed;

        public bool Moving { get; set; }

        public override PhysicalSize Size
        {
            get => new PhysicalSize
            {
                Height = 24,
                Width = 24
            };
            set { }
        }

        public override PhysicalPosition Position
        {
            get => new PhysicalPosition
            {
                X = base.Position.X + 4,
                Y = base.Position.Y + 4
            };
            set { }
        }

        public Point AttackRangeMultiples { get; set; }

        public string DamageSound { get; set; }

        public MapObject AttackRange => new MapObject
        {
            Position = new PhysicalPosition
            {
                X = this.Position.X - ((this.Size.Width * this.AttackRangeMultiples.X) - this.Size.Width) / 2,
                Y = this.Position.Y - ((this.Size.Height * this.AttackRangeMultiples.Y) - this.Size.Height) / 2
            },
            Size = new PhysicalSize
            {
                Width = this.Size.Width * AttackRangeMultiples.X,
                Height = this.Size.Height * AttackRangeMultiples.Y
            }
        };

        [ExcplicitFlowMethod]
        public void DamageExplicit(Damage damage)
        {
            this.Entity.Damage(damage);
        }

        //[FlowMethod]
        //public void Damage(bool forward)
        //{
        //    if (forward)
        //    {
        //        Global.AudioPlayer.Effect(DamageSound ?? "bat");
        //    }
        //    else
        //    {
        //        bool enemyDied = GetFlowProperty<bool>("EnemyDied");
        //        if (enemyDied)
        //        {
        //            Die?.Invoke();
        //            //death sound
        //            //Global.AudioPlayer.Effect(DamageSound ?? "bat");
        //        }
        //    }
        //}

        public override ISceneObject View(GameState gameState)
        {
            return new EnemySceneObject(gameState.Player, gameState.Map, this, this.TileSetRegion);
        }

        public long Exp => 5;
    }
}