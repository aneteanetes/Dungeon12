namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Abilities;
    using Rogue.Abilities.Enums;
    using Rogue.Classes;
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.SceneObjects.Effects;
    using Rogue.Drawing.SceneObjects.Gameplay;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Entites.Alive;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.Transactions;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PlayerSceneObject : AnimatedSceneObject<Avatar>
    {
        public override bool Shadow => true;

        public Avatar Avatar;

        public override int Layer => 1;

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
             ControlEventType.Focus,
              ControlEventType.Key
        };        

        private Character Player => Avatar.Character;
        private readonly GameMap location;

        public Action<Direction> OnStop;
        public Action OnStart;
        private Action<ISceneObject> destroyBinding;
        
        public PlayerSceneObject(Avatar player, GameMap location, Action<ISceneObject> destroyBinding)
            : base(null,player, player.Character.Name,new Rectangle
            {
                X = 32,
                Y = 0,
                Height = 32,
                Width = 32
            })
        {
            this.destroyBinding = destroyBinding;
            this.Avatar = player;
            this.location = location;
            this.Image = player.Tileset;
            this.Width = 1;
            this.Height = 1;
            this.AddChild(new ObjectHpBar(Player));

            this.abilities =new Lazy<Ability[]>(() => Avatar.Character.GetInstancesFromAssembly<Ability>().Select(x=>
            {
                x.Owner = player;
                return x;
            }).ToArray());

            player.StateAdded += s => RedrawStates(s);
            player.StateRemoved += s => RedrawStates(s, true);
            AddBuffs();
        }

        public double Speed => Avatar.MovementSpeed;

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
            foreach (var buff in Avatar.States)
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
                if (torchlight != null)
                {
                    torchlight.Left = 0;
                }

                this.Avatar.Location.Y -= Speed;
                SetAnimation(this.Player.MoveUp);
                if (!CheckMoveAvailable(Direction.Up))
                {
                    OnStop(Direction.Up);
                }
                else if (this.aliveTooltip != null)
                {
                    this.aliveTooltip.Left = this.Position.X;
                    this.aliveTooltip.Top = this.Position.Y - 0.8;
                }
            }
            if (NowMoving.Contains(Direction.Down))
            {
                if (torchlight != null)
                {
                    torchlight.Left = 0;
                }

                this.Avatar.Location.Y += Speed;
                SetAnimation(this.Player.MoveDown);
                if (!CheckMoveAvailable(Direction.Down))
                {
                    OnStop(Direction.Down);
                }
                else if (this.aliveTooltip != null)
                {
                    this.aliveTooltip.Left = this.Position.X;
                    this.aliveTooltip.Top = this.Position.Y - 0.8;
                }
            }
            if (NowMoving.Contains(Direction.Left))
            {
                if (torchlight != null)
                {
                    torchlight.Left = 0.4;
                }

                this.Avatar.Location.X -= Speed;
                SetAnimation(this.Player.MoveLeft);
                if (!CheckMoveAvailable(Direction.Left))
                {
                    OnStop(Direction.Left);
                }
                else if (this.aliveTooltip != null)
                {
                    this.aliveTooltip.Left = this.Position.X;
                    this.aliveTooltip.Top = this.Position.Y - 0.8;
                }
            }
            if (NowMoving.Contains(Direction.Right))
            {
                if (torchlight != null)
                {
                    torchlight.Left = 0.2;
                }

                this.Avatar.Location.X += Speed;
                SetAnimation(this.Player.MoveRight);
                if (!CheckMoveAvailable(Direction.Right))
                {
                    OnStop(Direction.Right);
                }
                else if (this.aliveTooltip != null)
                {
                    this.aliveTooltip.Left = this.Position.X;
                    this.aliveTooltip.Top = this.Position.Y - 0.8;
                }
            }
        }

        private static int movedRight = 0;

        private bool CheckMoveAvailable(Direction direction)
        {
            if (this.location.Move(Avatar, direction))
            {
                this.Left = Avatar.Location.X;
                this.Top = Avatar.Location.Y;

                this.OnMove?.Invoke();

                return true;
            }
            else
            {
                Avatar.Location.X = this.Left;
                Avatar.Location.Y = this.Top;

                return false;
            }
        }

        public Action OnMove;

        private HashSet<Direction> NowMoving = new HashSet<Direction>();

        public void StopMovings()
        {
            this.RequestStop();
            foreach (var moving in NowMoving)
            {
                OnStop(moving);
            }
            this.NowMoving.Clear();
        }

        public bool BlockMouse { get; set; }

        private HashSet<Direction> OppositeDirections = new HashSet<Direction>();

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

            base.KeyDown(key, modifier, hold);
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
                        OnStop(Direction.Right);
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
                        OnStop(Direction.Left);
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
                        OnStop(Direction.Up);
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
                        OnStop(Direction.Down);
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

        protected override Key[] KeyHandles => new Key[]
        {
            Key.D,
            Key.A,
            Key.W,
            Key.S,
            Key.LeftShift
        };

        public override void Focus()
        {
            //base.Focus();
        }

        public override void Unfocus()
        {
            //base.Unfocus();
        }

        private Lazy<Ability[]> abilities;

        public Ability[] GetAbilities() => abilities.Value;

        public Ability GetAbility(AbilityPosition abilityPosition) => this.GetAbilities().FirstOrDefault(x => x.AbilityPosition == abilityPosition);

        protected override void Action(MouseButton mouseButton) { }

        protected override void StopAction() { }

        public HashSet<MapObject> TargetsInFocus = new HashSet<MapObject>();

        private TorchlightInHandsSceneObject torchlight;
        private bool torch = false;
        public void Torchlight()
        {
            if (!torch)
            {
                AddTorchlight();
            }
            else
            {
                RemoveTorchlight();
            }

            torch = !torch;
        }

        private void AddTorchlight()
        {
            torchlight = new TorchlightInHandsSceneObject();
            this.AddChild(torchlight);
        }

        private void RemoveTorchlight()
        {
            this.RemoveChild(torchlight);
            torchlight?.Destroy?.Invoke();
        }
    }
}