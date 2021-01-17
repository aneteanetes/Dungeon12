using Dungeon;
using Dungeon.Drawing.Impl;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Physics;
using Dungeon.SceneObjects;
using Dungeon.Transactions;
using Dungeon.View.Interfaces;
using Dungeon12.Map;
using Dungeon12.Map.Objects;
using System;
using System.Collections.Generic;

namespace Dungeon12.Servant.Effects.Сonsecration
{
    public class СonsecrationCircle : ImageControl
    {
        public override bool AbsolutePosition => false;

        public override bool CacheAvailable => false;
        
        public СonsecrationCircle(GameMap gameMap, PhysicalObject position, long buffValue) : base("Effects/concentration.png".AsmImg())
        {
            var totem = new ConsecrationCircleTotem(position.Position.X / 32, position.Position.Y / 32, buffValue);
            gameMap.MapObject.Add(totem);

            this.Width = 3;
            this.Height = 1.5;

            this.Destroy += () =>
            {
                gameMap.MapObject.Remove(totem);
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

            Dungeon12.Global.Time.Timer(Guid.NewGuid().ToString())
                .After(millisec)
                .Do(() => this.Destroy?.Invoke())
                .Auto();

            return this;
        }

        private class CircleLight : EmptySceneObject
        {
            public CircleLight()
            {
                this.Width = 3;
                this.Height = 1.5;
                
                this.Effects = new List<IEffect>()
                {
                    new ParticleEffect()
                    {
                        Name="Сonsecration",
                        Scale = 1
                    }
                };
            }
        }
    }

    public class ConsecrationCircleTotem : Totem
    {
        public ConsecrationCircleTotem(double x, double y,long buffValue)
        {
            this.Location = new Dungeon.Types.Point(x, y);
            this.Location.X -= 1.5;
            this.Location.Y -= 0.75;

            this.Size = new Dungeon.Physics.PhysicalSize()
            {
                Height = 1.5 * 32,
                Width = 3 * 32
            };

            buff = new Lazy<Buff>(() => new Buff(buffValue));
        }

        private Lazy<Buff> buff;

        public override Applicable ApplicableEffect => buff.Value;

        public override bool CanAffect(MapObject @object) => @object is Avatar;

        private class Buff : Applicable
        {
            private long _buffValue;
            public Buff(long buffValue)
            {
                _buffValue = buffValue;
            }

            public void Apply(Avatar @object)
            {
                @object.Character.Defence += _buffValue;
                @object.Character.Barrier += _buffValue;
                base.Apply(@object);
            }

            public void Discard(Avatar @object)
            {
                @object.Character.Defence -= _buffValue;
                @object.Character.Barrier -= _buffValue;
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
