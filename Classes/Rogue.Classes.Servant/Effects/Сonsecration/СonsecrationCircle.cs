using Rogue.Drawing.Impl;
using Rogue.Drawing.SceneObjects;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Servant.Effects.Сonsecration
{
    public class СonsecrationCircle : ImageControl
    {
        public override bool AbsolutePosition => false;

        public override bool CacheAvailable => false;
        
        public СonsecrationCircle(GameMap gameMap, Physics.PhysicalObject position) : base("Effects/concentration.png".PathAsmImg())
        {
            var totem = new ConsecrationCircleTotem(position.Position.X / 32, position.Position.Y / 32);
            gameMap.Map.Add(totem);

            this.Width = 3;
            this.Height = 1.5;

            this.Destroy += () =>
            {
                gameMap.Map.Remove(totem);
                totem.Destroy?.Invoke();
            };
        }

        public СonsecrationCircle Init(double millisec)
        {
            this.AddChild(new CircleLight()
            {
                Left=1.5,
                Top=0.5
            });

            Global.Time.Timer(Guid.NewGuid().ToString())
                .After(millisec)
                .Do(() => this.Destroy?.Invoke())
                .Auto();

            return this;
        }

        private class CircleLight : SceneObject
        {
            public CircleLight()
            {
                this.Width = 3;
                this.Height = 1.5;
                
                this.Effects = new List<View.Interfaces.IEffect>()
                {
                    new ParticleEffect()
                    {
                        Name="Сonsecration",
                        Scale = 1,
                        Assembly="Rogue.Classes.Servant"
                    }
                };
            }
        }
    }

    public class ConsecrationCircleTotem : Totem
    {
        public ConsecrationCircleTotem(double x, double y)
        {
            this.Location = new Types.Point(x, y);
            this.Location.X -= 1.5;
            this.Location.Y -= 0.75;

            this.Size = new Physics.PhysicalSize()
            {
                Height = 1.5 * 32,
                Width = 3 * 32
            };
        }

        public override Applicable ApplicableEffect { get; } = new Buff();

        public override bool CanAffect(MapObject @object) => @object is Avatar;

        private class Buff : Applicable
        {
            public void Apply(Avatar @object)
            {
                @object.Character.Defence += 10;
                @object.Character.Barrier += 10;
                base.Apply(@object);
            }

            public void Discard(Avatar @object)
            {
                @object.Character.Defence -= 10;
                @object.Character.Barrier -= 10;
                base.Discard(@object);
            }

            protected override void CallApply(dynamic obj)
            {
                Apply(obj);
            }

            protected override void CallDiscard(dynamic obj)
            {
                Discard(obj);
            }
        }
    }
}
