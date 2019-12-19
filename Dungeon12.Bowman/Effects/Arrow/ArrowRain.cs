using Dungeon;
using Dungeon.Drawing.Impl;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon12.Entities.Alive;
using Dungeon12.Map;
using Dungeon12.Map.Objects;
using System.Collections.Generic;

namespace Dungeon12.Bowman.Effects
{
    public class ArrowRain : EmptySceneObject
    {
        GameMap _gameMap;

        public int CountPerSec { get; set; }

        public long MinDmg { get; set; }

        public long MaxDmg { get; set; }

        public int TimeMs { get; set; }

        private Bowman _bowman;

        public ArrowRain(Bowman bowman, long timeMS, Point pos, GameMap gameMap)
        {
            _bowman = bowman;
            _gameMap = gameMap;

            this.Left = pos.X - 1.5;
            this.Top = pos.Y - 0.5;
            this.Width = 3;
            this.Height = 3;

            this.Effects = new List<Dungeon.View.Interfaces.IEffect>()
                {
                    new ParticleEffect()
                    {
                        Name="RainOfArrow",
                        Scale = 0.09
                    }
                };

            Global.Time.Timer(nameof(ArrowRain))
                .After(timeMS)
                .Do(this.Destroy)
                .Trigger();
        }

        int frame = 0;

        public override void Update()
        {
            frame++;
            if (frame > 60)
            {
                frame = 0;

                var rangeObj = new MapObject()
                {
                    Size = new Dungeon.Physics.PhysicalSize()
                    {
                        Height = 3 * 32,
                        Width = 3 * 32
                    },
                    Position = new Dungeon.Physics.PhysicalPosition()
                    {
                        X = this.Left * 32,
                        Y = this.Top * 32
                    }
                };

                _gameMap.All<NPCMap>(rangeObj).ForEach(mob =>
                {
                    long Damage = RandomDungeon.Range(MinDmg, MaxDmg);
                    mob.Entity.Damage(_bowman,new Damage()
                    {
                        Amount=Damage,
                        Type=DamageType.Kenetic
                    });
                });
            }
        }
    }
}
