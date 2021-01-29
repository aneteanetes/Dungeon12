using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Control.Pointer;
using Dungeon.Types;
using Dungeon12.Drawing.SceneObjects.Map;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.GameObjects.Party
{
    public class PartySceneObject : MoveableSceneObject<Party>
    {
        private Character.CharacterEntity ActiveCharacter => Component.CharacterSlot1;

        public PartySceneObject(Party component) : base(component, component.CharacterSlot1, component.CharacterSlot1.DefaultFramePosition)
        {
            this.Image = component.CharacterSlot1.Image;
            this.Width = component.CharacterSlot1.DefaultFramePosition.Width;
            this.Height = component.CharacterSlot1.DefaultFramePosition.Height;
        }

        protected override void Action(MouseButton mouseButton)
        {
        }

        protected override void DrawLoop()
        {
            var _ = NowMoving.Count == 0
                ? RequestStop()
                : RequestResume();

            var distMoves = NowMoving
                .Distinct()
                .ToArray();

            var moveDirection = Direction.Idle;

            if (distMoves.Length > 0)
                moveDirection = distMoves.Aggregate((d1, d2) => (Direction)((int)d1 + (int)d2));

            switch (moveDirection)
            {
                case Direction.Up:
                    this.Top -= Component.CharacterSlot1.Speed;
                    SetImageForce(ActiveCharacter.Image);
                    SetAnimation(ActiveCharacter.MoveUp);
                    break;
                case Direction.Down:
                    this.Top += Component.CharacterSlot1.Speed;
                    SetImageForce(ActiveCharacter.Image);
                    SetAnimation(ActiveCharacter.MoveDown);
                    break;
                case Direction.Left:
                    this.Left -= Component.CharacterSlot1.Speed;
                    SetImageForce(ActiveCharacter.Image);
                    SetAnimation(ActiveCharacter.MoveLeft);
                    break;
                case Direction.Right:
                    this.Left += Component.CharacterSlot1.Speed;
                    SetImageForce(ActiveCharacter.Image);
                    SetAnimation(ActiveCharacter.MoveRight);
                    break;
                case Direction.UpLeft:
                    this.Top -= Component.CharacterSlot1.Speed;
                    this.Left -= Component.CharacterSlot1.Speed;
                    SetImageForce(ActiveCharacter.ImageAxis);
                    SetAnimation(ActiveCharacter.MoveUpLeft);
                    break;
                case Direction.UpRight:
                    this.Top -= Component.CharacterSlot1.Speed;
                    this.Left += Component.CharacterSlot1.Speed;
                    SetImageForce(ActiveCharacter.ImageAxis);
                    SetAnimation(ActiveCharacter.MoveUpRight);
                    break;
                case Direction.DownLeft:
                    this.Top += Component.CharacterSlot1.Speed;
                    this.Left -= Component.CharacterSlot1.Speed;
                    SetImageForce(ActiveCharacter.ImageAxis);
                    SetAnimation(ActiveCharacter.MoveDownLeft);
                    break;
                case Direction.DownRight:
                    this.Top += Component.CharacterSlot1.Speed;
                    this.Left += Component.CharacterSlot1.Speed;
                    SetImageForce(ActiveCharacter.ImageAxis);
                    SetAnimation(ActiveCharacter.MoveDownRight);
                    break;
                default:
                    break;
            }

            //if (NowMoving.Contains(Direction.Up))
            //{
            //    this.Top -= Component.CharacterSlot1.Speed;
            //    SetAnimation(ActiveCharacter.MoveUp);

            //    if (NowMoving.Contains(Direction.Left))
            //    {
            //        this.Left -= Component.CharacterSlot1.Speed;
            //        SetImageForce(ActiveCharacter.ImageAxis);
            //        SetAnimation(ActiveCharacter.MoveUpLeft);
            //    }
            //    else if (NowMoving.Contains(Direction.Right))
            //    {
            //        this.Left += Component.CharacterSlot1.Speed;
            //        SetImageForce(ActiveCharacter.ImageAxis);
            //        SetAnimation(ActiveCharacter.MoveUpLeft);
            //    }
            //    else SetImageForce(ActiveCharacter.Image);
            //}
            //if (NowMoving.Contains(Direction.Down))
            //{
            //    this.Top += Component.CharacterSlot1.Speed;
            //    SetAnimation(ActiveCharacter.MoveDown);
            //}
            //if (NowMoving.Contains(Direction.Left) && !NowMoving.Contains(Direction.Up))
            //{
            //    this.Left -= Component.CharacterSlot1.Speed;
            //    SetAnimation(ActiveCharacter.MoveLeft);
            //}
            //if (NowMoving.Contains(Direction.Right))
            //{
            //    this.Left += Component.CharacterSlot1.Speed;
            //    SetAnimation(ActiveCharacter.MoveRight);
            //}
        }

        #region Moving

        public bool BlockMoveInput { get; set; }

        private readonly HashSet<Direction> NowMoving = new HashSet<Direction>();

        private HashSet<Direction> OppositeDirections = new HashSet<Direction>();

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (BlockMoveInput)
                return;

            base.KeyDown(key, modifier, hold);

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
            if (BlockMoveInput)
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
            ControlEventType.LeftStickMove,
            ControlEventType.Key
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

        public override void LeftStickMove(Direction direction, Distance distance)
        {
            NowMoving.Clear();
            //if (!NowMoving.Contains(direction.Opposite()))
            {
                if(direction.ToString().Contains("Left") || direction.ToString().Contains("Right"))
                {
                    direction = direction.OppositeX();
                }

                NowMoving.Add(direction);
            }
        }

        public override void LeftStickMoveOnce(Direction direction, Distance distance)
        {
            NowMoving.Clear();
        }

        protected Direction DetectDirection(float x1, float x2, float y1, float y2)
        {
            Direction dirX = Direction.Idle;
            Direction dirY = Direction.Idle;

            if (x1 <= x2)
            {
                dirX = Direction.Left;
            }
            if (x1 >= x2)
            {
                dirX = Direction.Right;
            }

            if (y1 >= y2)
            {
                dirY = Direction.Down;
            }

            if (y1 <= y2)
            {
                dirY = Direction.Up;
            }

            return (Direction)((int)dirX + (int)dirY);
        }

        #endregion
    }
}
