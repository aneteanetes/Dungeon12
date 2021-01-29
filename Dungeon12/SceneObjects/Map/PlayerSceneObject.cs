namespace Dungeon12.Drawing.SceneObjects.Map
{
    using Dungeon12.Abilities;
    using Dungeon12.Abilities.Enums;
    using Dungeon12.Abilities.Talants.TalantTrees;
    using Dungeon12.Classes;
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon12.Drawing.SceneObjects.UI;
    using Dungeon.Entities.Animations;
    using Dungeon.Events;
    using Dungeon12.Map;
    using Dungeon12.Map.Objects;
    using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
    using Dungeon.Transactions;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dungeon12.Events;
    using Dungeon;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.Physics;
    using Dungeon.Drawing;

    public class PlayerSceneObject : MoveableSceneObject<Avatar>
    {
        public override Avatar Component => Avatar;

        public override bool Events => true;

        public override bool Shadow => true;

        protected override bool SilentTooltip => true;

        private Avatar _avatar;
        public Avatar Avatar
        {
            get => _avatar;
            set
            {
                _avatar = value;
                BindComponent(_avatar);
            }
        }

        public override int LayerLevel => 1;

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.Focus,
            ControlEventType.Key,
            ControlEventType.GlobalMouseMove
        };

        private Character Player => Avatar.Character;

        private GameMap Location => Global.GameState.Map;

        public Action<Direction> OnStop;

        [ExcplicitFlowMethod]
        private void OnStopFlow(Direction dir)
        {
            Avatar.OnMoveStop?.Invoke(dir);
            OnStop?.Invoke(dir);
        }


        public Action OnStart;
        private Action<ISceneObject> destroyBinding;

        public PlayerSceneObject(Avatar player, Action<ISceneObject> destroyBinding)
            : base(player, player.Entity,new Rectangle
            {
                X = 32,
                Y = 0,
                Height = 32,
                Width = 32
            })
        {
            this.destroyBinding = destroyBinding;
            this.Avatar = player;
            this.Width = 1;
            this.Height = 1;

            this.AddChild(new ObjectHpBar(Player));

            this.abilities = () => Avatar.Character.PropertiesOfType<Ability>();
            this.talantTrees = () => Avatar.Character.PropertiesOfType<TalantTree>();

            OnEvent(new ClassChangeEvent());
                      
            player.StateAdded += s => RedrawStates(s);
            player.StateRemoved += s => RedrawStates(s, true);
            AddBuffs();

            player.SceneObject = this;

            this.OnMove += () => this.Avatar.OnMove?.Invoke();

            this.AddChild(new ImageControl("ui/radius.png".AsmImg())
            {
                CacheAvailable=false,
                Left=-1,
                Top=-1
            });
        }

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }

        public virtual void OnEvent(ClassChangeEvent @event)
        {
            this.SetImageForce(this.Avatar.Tileset);
            this.GetAbilities().ForEach(a =>
            {
                a.Owner = this.Avatar;
            });
        }

        public double Speed => Avatar.MovementSpeed;

        private List<ImageControl> buffs = new List<ImageControl>();

        private void RedrawStates(Applicable applicable,bool remove=false)
        {
            if (applicable != null && remove)
            {
                var control = buffs.Find(x => x.Image == applicable.Image);
                if (control != default)
                {
                    control.Destroy?.Invoke();
                    buffs.Remove(control);
                    this.RemoveChild(control);
                }
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

        protected virtual void OnMoveRegistered(Direction dir) { }

        /// <summary>
        /// заморозить отрисовку
        /// <para>
        /// Требуется что бы при загрузке новый объект персонажа случайно не получил старую ссылку на карту
        /// </para>
        /// </summary>
        public bool FreezeDrawLoop { get; set; }

        private Action _movePointAction;
        private PhysicalObject _movePointTarget;

        public void BindMovePointAction(PhysicalObject target, Action action)
        {
            var text = new DrawText("Слишком далеко!", ConsoleColor.White) { Size = 10 };
            var left = this.Left - ((this.MeasureText(text).X / 32) / 5.5);
            this.ShowInScene?.Invoke(new PopupString(text, new Point(left, this.Top),speed:0.05).InList<ISceneObject>());            
#warning отключено взаимодействие мышкой потому что появляется проблема с камерой
            return;
            _movePointAction = action;
            _movePointTarget = target;
            move = DetectDirection(target);
        }

        public override void Update()
        {
            if (_movePointTarget == default)
                return;

            if (_movePointTarget?.IntersectsWithOrContains(this.Avatar) ?? false)
            {
                _movePointAction?.Invoke();
                _movePointAction = default;
                _movePointTarget = default;
            }

            Move(move);
        }

        protected override bool CheckMoveAvailable(Direction direction)
        {
            if (this.Location.Move(Avatar, direction))
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

        protected override void WhenMoveNotAvailable(Direction dir)
        {
            StopAutoMove();

            if (dir == Direction.UpLeft)
            {
                OnStopFlow(Direction.Up);
                OnStopFlow(Direction.Left);
            }
            else if (dir == Direction.UpRight)
            {
                OnStopFlow(Direction.Up);
                OnStopFlow(Direction.Right);
            }
            else if (dir == Direction.DownLeft)
            {
                OnStopFlow(Direction.Down);
                OnStopFlow(Direction.Left);
            }
            else if (dir == Direction.DownRight)
            {
                OnStopFlow(Direction.Down);
                OnStopFlow(Direction.Right);
            }
            else OnStopFlow(dir);


            Toast.Show("Невозможно пройти");
        }
        
        private void StopAutoMove()
        {
            move = Direction.Idle;
            _movePointTarget = default;
            _movePointAction = default;
        }

        protected override void DrawLoop()
        {
            if (FreezeDrawLoop)
                return;

            var _ = NowMoving.Count == 0
                ? RequestStop()
                : RequestResume();

            if (NowMoving.Contains(Direction.Up))
            {
                OnMoveRegistered(Direction.Up);

                this.Avatar.Location.Y -= Speed;
                if (!DontChangeVisionDirection)
                    SetAnimation(this.Player.MoveUp);
                if (!CheckMoveAvailable(Direction.Up))
                {
                    OnStopFlow(Direction.Up);
                }
                else if (this.aliveTooltip != null)
                {
                    this.aliveTooltip.Left = this.BoundPosition.X;
                    this.aliveTooltip.Top = this.BoundPosition.Y - 0.8;

                    if (!DontChangeVisionDirection)                    
                        Avatar.VisionDirection = Direction.Up;                    
                }
            }
            if (NowMoving.Contains(Direction.Down))
            {
                OnMoveRegistered(Direction.Down);

                this.Avatar.Location.Y += Speed;
                if (!DontChangeVisionDirection)
                    SetAnimation(this.Player.MoveDown);
                if (!CheckMoveAvailable(Direction.Down))
                {
                    OnStopFlow(Direction.Down);
                }
                else if (this.aliveTooltip != null)
                {
                    this.aliveTooltip.Left = this.BoundPosition.X;
                    this.aliveTooltip.Top = this.BoundPosition.Y - 0.8;
                    if (!DontChangeVisionDirection)
                        Avatar.VisionDirection = Direction.Down;
                }
            }
            if (NowMoving.Contains(Direction.Left))
            {
                OnMoveRegistered(Direction.Left);

                this.Avatar.Location.X -= Speed;
                if (!DontChangeVisionDirection)
                    SetAnimation(this.Player.MoveLeft);
                if (!CheckMoveAvailable(Direction.Left))
                {
                    OnStopFlow(Direction.Left);
                }
                else if (this.aliveTooltip != null)
                {
                    this.aliveTooltip.Left = this.BoundPosition.X;
                    this.aliveTooltip.Top = this.BoundPosition.Y - 0.8;
                    if (!DontChangeVisionDirection)
                        Avatar.VisionDirection = Direction.Left;
                }
            }
            if (NowMoving.Contains(Direction.Right))
            {
                OnMoveRegistered(Direction.Right); 

                this.Avatar.Location.X += Speed;
                if (!DontChangeVisionDirection)
                    SetAnimation(this.Player.MoveRight);
                if (!CheckMoveAvailable(Direction.Right))
                {
                    OnStopFlow(Direction.Right);
                }
                else if (this.aliveTooltip != null)
                {
                    this.aliveTooltip.Left = this.BoundPosition.X;
                    this.aliveTooltip.Top = this.BoundPosition.Y - 0.8;
                    if (!DontChangeVisionDirection)
                        Avatar.VisionDirection = Direction.Right;
                }
            }
        }

        protected override void AnimationLoop()
        {
            //Global.AudioPlayer.Effect("step.wav".AsmSoundRes(), new Dungeon.Audio.AudioOptions()
            //{
            //    Volume = 0.05
            //});
        }

        public Action OnMove;

        private readonly HashSet<Direction> NowMoving = new HashSet<Direction>();

        public void MoveStep(Direction dir, bool remove, bool dontChangeVisionDirection, bool disableCameraAffect, bool blockMoveInput)
        {
            this.DontChangeVisionDirection = dontChangeVisionDirection;
            if (!remove)
            {
                Avatar.CameraAffect = disableCameraAffect;
                this.BlockMoveInput = blockMoveInput;
                this.NowMoving.Add(dir);
            }
            else
            {
                Avatar.CameraAffect = true;
                Avatar.OnMoveStop?.Invoke(dir);
                this.NowMoving.Remove(dir);
                DontChangeVisionDirection = false;
                SwitchPlayerFace(dir.Opposite());
                this.BlockMoveInput = false;
            }
        }

        public bool BlockMoveInput { get; set; }

        private bool DontChangeVisionDirection { get; set; } = false;

        public void StopMovings()
        {
            this.RequestStop();
            var m = new HashSet<Direction>(NowMoving);
            NowMoving.Clear();
            foreach (var moving in m)
            {
                OnStopFlow(moving);
            }
        }

        public bool BlockMouse { get; set; }

        private HashSet<Direction> OppositeDirections = new HashSet<Direction>();

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (BlockMoveInput)
                return;

            base.KeyDown(key, modifier, hold);

            StopAutoMove();

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
                case Key.Q:
                case Key.E:
                    UseAbility();
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
                        OnStopFlow(Direction.Right);
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
                        OnStopFlow(Direction.Left);
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
                        OnStopFlow(Direction.Up);
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
                        OnStopFlow(Direction.Down);
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
            Key.E,
            Key.Q,
            Key.LeftShift
        };
        
        private Direction _directionVision = Direction.Idle;
        private Direction DirectionVision
        {
            get => _directionVision;
            set
            {
                if (NowMoving.Count == 0 && _directionVision != value)
                {
                    _directionVision = value;
                    SwitchPlayerFace(value);
                }
            }
        }

        private void SwitchPlayerFace(Direction value)
        {
            Animation animap = null;
            switch (value)
            {
                case Direction.Up:
                    animap = this.Player.MoveUp;
                    break;
                case Direction.Down:
                    animap = this.Player.MoveDown;
                    break;
                case Direction.Left:
                    animap = this.Player.MoveLeft;
                    break;
                case Direction.Right:
                    animap = this.Player.MoveRight;
                    break;
                default:
                    break;
            }

            SetAnimation(animap);
            FramePosition.Pos = animap.Frames[0];
            Avatar.VisionDirection = value;
        }

        public override void GlobalMouseMove(PointerArgs args)
        {
            var pointGameCoord = args.GameCoordinates;
            var x = pointGameCoord.X;
            var y = pointGameCoord.Y;

            if (x < this.Left)
            {
                DirectionVision = Direction.Left;
            }

            if (x > this.Left)
            {
                DirectionVision = Direction.Right;
            }

            if (y > this.Top+2)
            {
                DirectionVision = Direction.Down;
            }

            if (y < this.Top-2)
            {
                DirectionVision = Direction.Up;
            }
        }

        private Func<Ability[]> abilities;

        public Ability[] GetAbilities() => abilities?.Invoke();


        private Func<TalantTree[]> talantTrees;
        public List<TalantTree> GetTalantTrees() => talantTrees?.Invoke().ToList();

        public Ability GetAbility(AbilityPosition abilityPosition) => this.GetAbilities().FirstOrDefault(x => x.AbilityPosition == abilityPosition);
        
        protected override void Action(MouseButton mouseButton) => UseAbility();

        private void UseAbility()
        {
            if(!InFocus)
            {
                return;
            }

            if (ability == default)
                return;

            if (ability.TargetType == AbilityTargetType.TargetFrendly)
            {
                var avatar = playerSceneObject.Avatar;
                ability.Target = this.Avatar.Character;
                if (ability.CastAvailableCooldown(avatar))
                {
                    ability.CastCooldown(Location, avatar);
                }
            }
        }

        protected override void StopAction() { }

        public HashSet<MapObject> TargetsInFocus = new HashSet<MapObject>();
    }
}