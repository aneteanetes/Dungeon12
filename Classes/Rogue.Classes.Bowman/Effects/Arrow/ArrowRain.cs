using Rogue.Drawing.Impl;
using Rogue.Drawing.SceneObjects;
using Rogue.Entites.Enemy;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Bowman.Effects
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

            this.Effects = new List<View.Interfaces.IEffect>()
                {
                    new ParticleEffect()
                    {
                        Name="RainOfArrow",
                        Scale = 0.09,
                        Assembly="Rogue.Classes.Bowman"
                    }
                };

            Global.Time.Timer(nameof(ArrowRain))
                .Each(timeMS)
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
                    Size = new Physics.PhysicalSize()
                    {
                        Height = 3 * 32,
                        Width = 3 * 32
                    },
                    Position = new Physics.PhysicalPosition()
                    {
                        X = this.Left * 32,
                        Y = this.Top * 32
                    }
                };

                _gameMap.All<Mob>(rangeObj).ForEach(mob =>
                {
                    long Damage = RandomRogue.Range(2, 11);
                    mob.Flow(t => t.Damage(true), new { Damage });
                });
            }
        }
    }
}
