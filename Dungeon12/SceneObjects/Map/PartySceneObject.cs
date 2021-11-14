namespace Dungeon12.Drawing.SceneObjects.Map
{
    using Dungeon;
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon.SceneObjects;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Dungeon12.Entities;
    using System.Collections.Generic;

    public class PartySceneObject : SceneControl<Party>
    {
        public override bool Events => true;

        public override bool Shadow => true;

        public override bool Updatable => base.Updatable;

        public override bool CacheAvailable => false;

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.Focus,
            ControlEventType.Key,
            ControlEventType.GlobalMouseMove
        };

        private HeroSceneObject Hero1 { get; set; }

        /// <summary>
        /// left from main
        /// </summary>
        private HeroSceneObject Hero2 { get; set; }

        /// <summary>
        /// Right from main
        /// </summary>
        private HeroSceneObject Hero3 { get; set; }

        /// <summary>
        /// Always back
        /// </summary>
        private HeroSceneObject Hero4 { get; set; }

        private HeroSceneObject[] Heroes { get; set; }

        public PartySceneObject(Party party) : base(party)
        {
            Hero1 = new HeroSceneObject(party.Hero1)
            {
                LayerLevel = 4
            };
            Hero2 = new HeroSceneObject(party.Hero2)
            {
                LayerLevel = 1
            };
            Hero3 = new HeroSceneObject(party.Hero3)
            {
                LayerLevel = 2
            };
            Hero4 = new HeroSceneObject(party.Hero4)
            {
                LayerLevel = 3
            };

            this.AddChild(Hero2);
            this.AddChild(Hero3);
            this.AddChild(Hero4);
            this.AddChild(Hero1);
            SetHeroesInParty(Direction.Down);

            Heroes = new HeroSceneObject[] { Hero1, Hero2, Hero3, Hero4 };
        }

        private void SetHeroesInParty(Direction direction)
        {
            Reset();

            if (direction == Direction.Down)
            {
                Hero2.Left -= 20;
                Hero2.Top -= 16;

                Hero3.Left += 20;
                Hero3.Top -= 16;

                Hero4.Top -= 24;
                Hero4.ZIndex = -1;

                Hero4.LayerLevel = 3;
                Hero1.LayerLevel = 4;
            }

            if (direction == Direction.Up)
            {
                Hero2.Left -= 20;
                Hero2.Top += 16;

                Hero3.Left += 20;
                Hero3.Top += 16;

                Hero4.Top += 24;

                Hero4.LayerLevel = 4;
                Hero1.LayerLevel = 3;
            }

            if (direction == Direction.Left)
            {
                Hero2.Left += 20;
                Hero2.Top -= 16;

                Hero3.Left += 20;
                Hero3.Top += 16;

                Hero4.Left += 40;
            }

            if (direction == Direction.Right)
            {
                Hero2.Left -= 20;
                Hero2.Top -= 16;

                Hero3.Left -= 20;
                Hero3.Top += 16;

                Hero4.Left -= 40;
            }
        }

        private void Reset()
        {
            Hero2.Left = 0;
            Hero2.Top = 0;
            Hero3.Left = 0;
            Hero3.Top = 0;
            Hero4.Left = 0;
            Hero4.Top = 0;
        }

        public override double Left => Component.Hero1.PhysicalObject.Position.X;

        public override double Top => Component.Hero1.PhysicalObject.Position.Y;

        public override double Height => Component.Hero1.WalkSpriteSheet.Height;

        public override double Width => Component.Hero1.WalkSpriteSheet.Width;

        public override void UpdateSceneObject(GameTimeLoop gameTime)
        {
            int dir = (int)Direction.Idle;

            if (NowMoving.Contains(Direction.Up))
            {
                dir += 1;
            }
            if (NowMoving.Contains(Direction.Down))
            {
                dir += 20;
            }
            if (NowMoving.Contains(Direction.Left))
            {
                dir += 300;
            }
            if (NowMoving.Contains(Direction.Right))
            {
                dir += 4000;
            }

            Direction direction = (Direction)dir;
            if (direction != Direction.Idle)
            {
                if (Component.Hero1.PhysicalObject.Move(direction))
                {
                    Component.Hero2?.PhysicalObject.MoveThrough(direction);
                    Component.Hero3?.PhysicalObject.MoveThrough(direction);
                    Component.Hero4?.PhysicalObject.MoveThrough(direction);

                    this.PlayAnimations(direction);
                }
            }
            else
            {
                this.StopAnimations();
            }
        }

        private void PlayAnimations(Direction direction)
        {
            if (direction == Direction.DownLeft || direction == Direction.DownRight)
                direction = Direction.Down;
            else if (direction == Direction.UpLeft || direction == Direction.UpRight)
                direction = Direction.Up;

            SetHeroesInParty(direction);

            var animName = direction.ToStringX().ToLowerInvariant();

            foreach (var hero in Heroes)
            {
                if (hero.Component != default)
                {
                    hero.Component.WalkSpriteSheet.Animations.TryGetValue(animName, out var anim);
                    hero.PlayAnimation(anim);
                }
            }
        }

        private void StopAnimations()
        {
            foreach (var hero in Heroes)
            {
                hero.StopAnimation();
            }
        }

        private readonly HashSet<Direction> NowMoving = new HashSet<Direction>();

        private readonly HashSet<Direction> OppositeDirections = new HashSet<Direction>();

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            switch (key)
            {
                case Key.D:
                    {
                        if (!NowMoving.Contains(Direction.Left))
                        {
                            NowMoving.Add(Direction.Right);
                        }
                        else
                        {
                            OppositeDirections.Add(Direction.Right);
                        }
                        break;
                    }
                case Key.A:
                    if (!NowMoving.Contains(Direction.Right))
                    {
                        NowMoving.Add(Direction.Left);
                    }
                    else
                    {
                        OppositeDirections.Add(Direction.Left);
                    }
                    break;
                case Key.W:
                    if (!NowMoving.Contains(Direction.Down))
                    {
                        NowMoving.Add(Direction.Up);
                    }
                    else
                    {
                        OppositeDirections.Add(Direction.Up);
                    }
                    break;
                case Key.S:
                    if (!NowMoving.Contains(Direction.Up))
                    {
                        NowMoving.Add(Direction.Down);
                    }
                    else
                    {
                        OppositeDirections.Add(Direction.Down);
                    }
                    break;
                default: break;
            }

        }

        public override void KeyUp(Key key, KeyModifiers modifier)
        {
            switch (key)
            {
                case Key.D:
                    {
                        if (OppositeDirections.Contains(Direction.Right))
                        {
                            OppositeDirections.Remove(Direction.Right);
                        }

                        NowMoving.Remove(Direction.Right);
                        if (OppositeDirections.Contains(Direction.Left))
                        {
                            OppositeDirections.Remove(Direction.Left);
                            NowMoving.Add(Direction.Left);
                        }
                    }
                    break;
                case Key.A:
                    {
                        if (OppositeDirections.Contains(Direction.Left))
                        {
                            OppositeDirections.Remove(Direction.Left);
                        }

                        NowMoving.Remove(Direction.Left);
                        if (OppositeDirections.Contains(Direction.Right))
                        {
                            OppositeDirections.Remove(Direction.Right);
                            NowMoving.Add(Direction.Right);
                        }
                    }
                    break;
                case Key.W:
                    {
                        if (OppositeDirections.Contains(Direction.Up))
                        {
                            OppositeDirections.Remove(Direction.Up);
                        }

                        NowMoving.Remove(Direction.Up);
                        if (OppositeDirections.Contains(Direction.Down))
                        {
                            OppositeDirections.Remove(Direction.Down);
                            NowMoving.Add(Direction.Down);
                        }
                    }
                    break;
                case Key.S:
                    {
                        if (OppositeDirections.Contains(Direction.Down))
                        {
                            OppositeDirections.Remove(Direction.Down);
                        }

                        NowMoving.Remove(Direction.Down);
                        if (OppositeDirections.Contains(Direction.Up))
                        {
                            OppositeDirections.Remove(Direction.Up);
                            NowMoving.Add(Direction.Up);
                        }
                    }
                    break;
                default: break;
            }

            if (NowMoving.Count == 0)
                StopAnimation();
        }

        protected override Key[] KeyHandles => new Key[]
        {
            Key.W,
            Key.A,
            Key.S,
            Key.D,
        };
    }
}