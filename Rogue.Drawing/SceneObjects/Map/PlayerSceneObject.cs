namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Keys;
    using Rogue.Drawing.SceneObjects.Gameplay;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Entites.Alive.Character;
    using Rogue.Map;
    using Rogue.Transactions;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public class PlayerSceneObject : AnimatedSceneObject
    {
        private Rogue.Map.Objects.Avatar avatar;
        private Player Player => avatar.Character;
        private readonly GameMap location;
        private readonly Action<ISceneObject> destroyBinding;
        private readonly Action<ISceneObject> publishBinding;

        public Action OnStop;
        public Action OnStart;

        public PlayerSceneObject(Rogue.Map.Objects.Avatar player, GameMap location, Action<ISceneObject> destroyBinding, Action<ISceneObject> publishBinding)
            : base(new Rectangle
            {
                X = 32,
                Y = 0,
                Height = 32,
                Width = 32
            })
        {
            this.publishBinding = publishBinding;
            this.destroyBinding = destroyBinding;
            this.avatar = player;
            this.location = location;
            this.Image = player.Tileset;
            this.Width = 1;
            this.Height = 1;
            this.AddChild(new ObjectHpBar(Player));

            player.StateAdded += s => RedrawStates(s);
            player.StateRemoved += s => RedrawStates(s, true);
            AddBuffs();
        }

        public double Speed => avatar.MovementSpeed;

        private List<ImageControl> buffs = new List<ImageControl>();

        private void RedrawStates(Applicable applicable,bool remove=false)
        {
            if (applicable != null && remove)
            {
                var control = buffs.Find(x => x.Image == applicable.Image);
                control.Destroy?.Invoke();
                buffs.Remove(control);
                this.RemoveChild(control);
                return;
            }

            AddBuff(applicable);
        }

        private void AddBuffs()
        {
            foreach (var buff in avatar.States)
            {
                AddBuff(buff);
            }
        }

        private void AddBuff(Applicable applicable)
        {
            var newBuff = new BuffSceneObject(applicable)
            {
                Left = buffs.Count * 0.5
            };
            newBuff.Top = - 0.75;

            newBuff.Destroy = () =>
            {
                destroyBinding(newBuff);
            };

            buffs.Add(newBuff);

            this.AddChild(newBuff);
        }

        protected override void DrawLoop()
        {
            var _ = NowMoving.Count == 0
                ? RequestStop()
                : RequestResume();

            if (NowMoving.Contains(Direction.Up))
            {
                this.avatar.SceenPosition.Y -= Speed;
                this.avatar.Location.Y -= Speed;
                SetAnimation(this.Player.MoveUp);
                if (!CheckMoveAvailable(Direction.Up))
                {
                    OnStop();
                }
                else
                {
                }
            }
            if (NowMoving.Contains(Direction.Down))
            {
                this.avatar.SceenPosition.Y += Speed;
                this.avatar.Location.Y += Speed;
                SetAnimation(this.Player.MoveDown);
                if (!CheckMoveAvailable(Direction.Down))
                {
                    OnStop();
                }
                else
                {
                }
            }
            if (NowMoving.Contains(Direction.Left))
            {
                this.avatar.SceenPosition.X -= Speed;
                this.avatar.Location.X -= Speed;
                SetAnimation(this.Player.MoveLeft);
                if (!CheckMoveAvailable(Direction.Left))
                {
                    OnStop();
                }
                else
                {
                }
            }
            if (NowMoving.Contains(Direction.Right))
            {
                this.avatar.SceenPosition.X += Speed;
                this.avatar.Location.X += Speed;
                SetAnimation(this.Player.MoveRight);
                if (!CheckMoveAvailable(Direction.Right))
                {
                    OnStop();
                }
                else
                {
                }
            }
        }

        private bool CheckMoveAvailable(Direction direction)
        {
            if (this.location.Move(avatar, direction))
            {
                this.Left = avatar.Location.X;
                this.Top = avatar.Location.Y;
                return true;
            }
            else
            {
                avatar.Location.X = this.Left;
                avatar.Location.Y = this.Top;
                return false;
            }
        }

        private HashSet<Direction> NowMoving = new HashSet<Direction>();

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            switch (key)
            {
                case Key.D: NowMoving.Add(Direction.Right); break;
                case Key.A: NowMoving.Add(Direction.Left); break;
                case Key.W: NowMoving.Add(Direction.Up); break;
                case Key.S: NowMoving.Add(Direction.Down); break;
                default: break;
            }
        }

        public override void KeyUp(Key key, KeyModifiers modifier)
        {
            switch (key)
            {
                case Key.D:
                    NowMoving.Remove(Direction.Right);
                    OnStop(); break;
                case Key.A:
                    NowMoving.Remove(Direction.Left);
                    OnStop(); break;
                case Key.W:
                    NowMoving.Remove(Direction.Up);
                    OnStop(); break;
                case Key.S:
                    NowMoving.Remove(Direction.Down);
                    OnStop(); break;
                default: break;
            }
        }

        protected override Key[] KeyHandles => new Key[]
        {
            Key.D,
            Key.A,
            Key.W,
            Key.S,
        };
    }
}