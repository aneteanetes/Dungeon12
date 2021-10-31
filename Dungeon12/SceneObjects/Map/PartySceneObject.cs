using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon12.Entities;
using System;
using System.Collections.Generic;

namespace Dungeon12.SceneObjects.Map
{
    public class PartySceneObject : SceneControl<Party>
    {
        public PartySceneObject(Party component) : base(component, true)
        {
            Image = component.Tileset;
            ImageRegion = component.TileSetRegion;
        }

        public override bool CacheAvailable => false;

        public override bool CachePosition => false;

        public override double Left => Component.Position.X;

        public override double Top => Component.Position.Y;

        public override double Width => Component.Size.Width;

        public override double Height => Component.Size.Height;

        private readonly HashSet<Direction> NowMoving = new HashSet<Direction>();
        private HashSet<Direction> OppositeDirections = new HashSet<Direction>();

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (Component.CantMove)
                return;

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
            if (Component.CantMove)
                return;

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
            base.KeyUp(key, modifier);
        }

        protected override ControlEventType[] Handles => new ControlEventType[]
            {
                ControlEventType.Key,
                ControlEventType.GlobalMouseMove
            };

        protected override Key[] KeyHandles => new Key[]
        {
            Key.D,
            Key.A,
            Key.W,
            Key.S,
            Key.E,
            Key.Q,
            Key.LeftShift
        };

        private Direction _directionVision = Direction.Idle;
        private void SetDirectionVision(Direction value)
        {
            if (NowMoving.Count == 0 && _directionVision != value)
            {
                _directionVision = value;
                SwitchPlayerFace(value);
            }
        }

        public override void GlobalMouseMove(PointerArgs args)
        {
            var pointGameCoord = args.GameCoordinates;
            var x = pointGameCoord.X;
            var y = pointGameCoord.Y;

            if (x < this.Left)
            {
                SetDirectionVision(Direction.Left);
            }

            if (x > this.Left)
            {
                SetDirectionVision(Direction.Right);
            }

            if (y > this.Top + 2)
            {
                SetDirectionVision(Direction.Down);
            }

            if (y < this.Top - 2)
            {
                SetDirectionVision(Direction.Up);
            }
        }

        private void SwitchPlayerFace(Direction value)
        {
            //switch (value)
            //{
            //    case Direction.Up:
            //        animap = this.Player.MoveUp;
            //        break;
            //    case Direction.Down:
            //        animap = this.Player.MoveDown;
            //        break;
            //    case Direction.Left:
            //        animap = this.Player.MoveLeft;
            //        break;
            //    case Direction.Right:
            //        animap = this.Player.MoveRight;
            //        break;
            //    default:
            //        break;
            //}

            //SetAnimation(animap);
            //FramePosition.Pos = animap.Frames[0];
            //Avatar.VisionDirection = value;
        }

        Action OnFrameUpdate;

        protected override void UpdateFrame()
        {
            OnFrameUpdate?.Invoke();
        }

        public override void Update(GameTimeLoop gameTime)
        {
            base.Update(gameTime);

            if (Component.CantMove || this.InAnimation)
                return;

            if (NowMoving.Count == 0)
            {
                this.StopAnimation();
            }

            if (NowMoving.Contains(Direction.Up))
            {
                if (CheckMoveAvailable(Direction.Up))
                {
                    OnFrameUpdate = () => Component.Position.Y -= Component.Speed;
                    this.PlayAnimation(Component.AnimationMap.Up());
                }
            }
            if (NowMoving.Contains(Direction.Down))
            {
                if (CheckMoveAvailable(Direction.Down))
                {
                    OnFrameUpdate = () => Component.Position.Y += Component.Speed;
                    this.PlayAnimation(Component.AnimationMap.Down());
                }
            }
            if (NowMoving.Contains(Direction.Left))
            {
                if (CheckMoveAvailable(Direction.Left))
                {
                    OnFrameUpdate = () => Component.Position.X -= Component.Speed;
                    this.PlayAnimation(Component.AnimationMap.Left());
                }
            }
            if (NowMoving.Contains(Direction.Right))
            {
                if (CheckMoveAvailable(Direction.Right))
                {
                    OnFrameUpdate = () => Component.Position.X += Component.Speed;
                    this.PlayAnimation(Component.AnimationMap.Right());
                }
            }
        }

        protected bool CheckMoveAvailable(Direction direction)
        {
            return true;
        }
    }
}
