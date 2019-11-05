using Dungeon;
using Dungeon.Drawing.Impl;
using Dungeon.Entites.Enemy;
using Dungeon.Map;
using Dungeon.Map.Objects;
using Dungeon.SceneObjects;
using Dungeon.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Bowman.Effects
{
    public class ArrowRain : SceneObject
    {
        GameMap _gameMap;

        public int CountPerSec { get; set; }

        public int MinDmg { get; set; }

        public int MaxDmg { get; set; }

        public int TimeMs { get; set; }

        public ArrowRain(long timeMS, Point pos, GameMap gameMap)
        {
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

            Dungeon.Global.Time.Timer(nameof(ArrowRain))
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

                _gameMap.All<Mob>(rangeObj).ForEach(mob =>
                {
                    long Damage = RandomDungeon.Range(2, 11);
                    mob.Flow(t => t.Damage(true), new { Damage });
                });
            }
        }
    }
}
