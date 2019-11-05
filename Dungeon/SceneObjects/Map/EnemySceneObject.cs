namespace Dungeon.Drawing.SceneObjects.Map
{
    using Dungeon.Abilities.Enums;
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon.Drawing.SceneObjects.UI;
    using Dungeon.Entites.Alive;
    using Dungeon.Entites.Animations;
    using Dungeon.Map;
    using Dungeon.Map.Objects;
    using Dungeon.SceneObjects;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;
    using RandomDungeon = Dungeon.RandomDungeon;

    public class EnemySceneObject : AnimatedSceneObject<Mob>
    {
        public Mob MobObj { get; set; }

        private readonly GameMap location;

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }

        public override string Cursor => "attackmeele";

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.Click,
            ControlEventType.Focus,
            ControlEventType.Key
        };

        protected override Key[] KeyHandles => new Key[]
        {
            Key.Q,
            Key.E,
            Key.LeftShift
        };

        public EnemySceneObject(PlayerSceneObject playerSceneObject, GameMap location, Mob mob, Rectangle defaultFramePosition)
            : base(playerSceneObject, mob, mob.Name, defaultFramePosition)
        {
            this.location = location;
            this.MobObj = mob;
            mob.SetParentFlow(this);

            this.Image = mob.Tileset;
            Left = mob.Location.X;
            Top = mob.Location.Y;
            Width = 1;
            Height = 1;

            mob.Die += () =>
            {
                this.Destroy?.Invoke();
            };

            if (mob.Enemy.Idle != null)
            {
                this.SetAnimation(mob.Enemy.Idle);
            }

            this.AddChild(new ObjectHpBar(mob.Enemy));

            attackTimer = new Timer(1000);
            attackTimer.AutoReset = false;
            attackTimer.Elapsed += (x, y) => attackAvailable = true;
        }

        protected override void DrawLoop()
        {
            if (moveDistance != 0)
            {
                moveDistance--;
                foreach (var move in moves)
                {
                    Move(DirectionMap[move]);
                }
            }
        }

        protected override void AnimationLoop()
        {
            if (!MobObj.Enemy.Aggressive)
            {
                return;
            }

            if (moveDistance == 0)
            {
                this.MobObj.IsChasing = false;
            }

            var players = location.Map.Query(this.MobObj.Vision, true)
                .SelectMany(nodes => nodes.Nodes)
                .Where(node => this.MobObj.Vision.IntersectsWith(node))
                .Where(x => typeof(Avatar).IsAssignableFrom(x.GetType()));

            if (players.Count() > 0)
            {
                var player = players.First();
                if (player.IntersectsWith(this.MobObj.AttackRange))
                {
                    if (attackAvailable)
                    {
                        Attack(player as Avatar);
                        attackAvailable = false;
                        attackTimer.Start();
                    }
                }

                if (!this.MobObj.IsChasing)
                {
                    moves.Clear();
                    moveDistance = 0;
                    Chasing(player as Avatar);
                }
            }

            if (moveDistance == 0)
            {
                moves.Clear();
                var next = RandomDungeon.Next(0, 10);
                if (next > 5)
                {
                    var direction = RandomDungeon.Next(0, 4);
                    moves.Add(direction);

                    var diagonally = RandomDungeon.Next(0, 4);
                    if (diagonally != direction && NotPair(direction, diagonally))
                    {
                        moves.Add(diagonally);
                    }

                    moveDistance = RandomDungeon.Next(100, 300);
                }
            }
        }

        private Timer attackTimer;

        private bool attackAvailable = true;

        private void Attack(Avatar avatar)
        {
            var player = avatar.Character;

            var value = (long)RandomDungeon.Next(2, 7);

            var dmg = value - player.Defence;

            if (dmg < 0)
            {
                dmg = 0;
            }

            player.HitPoints -= dmg;

            if (player.HitPoints <= 0)
            {
                avatar.Die?.Invoke();
            }

            var critical = dmg > 6;

            this.ShowEffects(new List<ISceneObject>()
                {
                    new PopupString(dmg.ToString()+(critical ? "!" : ""), critical ? ConsoleColor.Red : ConsoleColor.White,avatar.Location,25,critical ? 14 : 12,0.06)
                });
        }

        private void Chasing(Avatar avatar)
        {
            this.MobObj.IsChasing = true;
            if (avatar.Position.X <= this.MobObj.Position.X)
            {
                this.moves.Add(2);
            }
            if (avatar.Position.X >= this.MobObj.Position.X)
            {
                this.moves.Add(3);
            }
            if (avatar.Position.Y >= this.MobObj.Position.Y)
            {
                this.moves.Add(1);
            }
            if (avatar.Position.Y <= this.MobObj.Position.Y)
            {
                this.moves.Add(0);
            }
            moveDistance = 60;
        }

        private bool NotPair(int common, int additional)
        {
            if (common + additional == 1)
                return false;

            if (common + additional == 5)
                return false;

            return true;
        }

        private int moveDistance = 0;
        private readonly HashSet<int> moves = new HashSet<int>();

        private void Move((Direction dir, Vector vect, Func<Moveable, AnimationMap> anim) data)
        {
            switch (data.dir)
            {
                case Direction.Up:
                case Direction.Down:
                    switch (data.vect)
                    {
                        case Vector.Plus:
                            this.MobObj.Location.Y += this.MobObj.MovementSpeed;
                            break;
                        case Vector.Minus:
                            this.MobObj.Location.Y -= this.MobObj.MovementSpeed;
                            break;
                        default:
                            break;
                    }
                    break;
                case Direction.Left:
                case Direction.Right:
                    switch (data.vect)
                    {
                        case Vector.Plus:
                            this.MobObj.Location.X += this.MobObj.MovementSpeed;
                            break;
                        case Vector.Minus:
                            this.MobObj.Location.X -= this.MobObj.MovementSpeed;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            SetAnimation(data.anim(this.MobObj.Enemy));
            if (!CheckMoveAvailable(data.dir))
            {
                moveDistance = 0;
            }
            else if (this.aliveTooltip != null)
            {
                this.aliveTooltip.Left = this.Position.X;
                this.aliveTooltip.Top = this.Position.Y - 0.8;
            }
        }

        private bool CheckMoveAvailable(Direction direction)
        {
            var movingToSafe = !this.location.InSafe(this.MobObj);

            if (movingToSafe && this.location.Move(this.MobObj, direction))
            {
                this.Left = this.MobObj.Location.X;
                this.Top = this.MobObj.Location.Y;
                return true;
            }
            else
            {
                this.MobObj.Location.X = this.Left;
                this.MobObj.Location.Y = this.Top;
                return false;
            }
        }

        private readonly Dictionary<int, (Direction dir, Vector vect, Func<Moveable, AnimationMap> anim)> DirectionMap = new Dictionary<int, (Direction, Vector, Func<Moveable, AnimationMap>)>
        {
            { 0, (Direction.Up, Vector.Minus, m=>m.MoveUp) },
            { 1,(Direction.Down, Vector.Plus, m=>m.MoveDown) },
            { 2,(Direction.Left, Vector.Minus, m=>m.MoveLeft) },
            { 3,(Direction.Right, Vector.Plus, m=>m.MoveRight) },
        };

        private enum Vector
        {
            Plus,
            Minus
        }

        public override void Focus()
        {
            playerSceneObject.TargetsInFocus.Add(MobObj);
            base.Focus();
        }

        public override void Unfocus()
        {
            playerSceneObject.TargetsInFocus.Remove(MobObj);
            base.Unfocus();
        }

        protected override bool CheckActionAvailable(MouseButton mouseButton)
        {
            if (mouseButton != MouseButton.None)
            {
                var range = ability.Range;
                return this.MobObj.IntersectsWith(range);
            }

            //вот тут ещё отмена Changell навыка будет

            return false;
        }

        protected override void Action(MouseButton mouseButton) => UseAbility();

        protected override void StopAction() { }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            base.KeyDown(key, modifier, hold);
            if (key == Key.Q || key == Key.E)
            {
                UseAbility();
            }
        }

        private void UseAbility()
        {
            if (ability == default)
                return;

            if (ability.TargetType == AbilityTargetType.Target || ability.TargetType == AbilityTargetType.TargetAndNonTarget)
            {
                var avatar = playerSceneObject.Avatar;
                ability.Target = this.@object.Enemy;
                if (ability.CastAvailableCooldown(avatar))
                {
                    ability.CastCooldown(location, avatar);
                }
            }
        }

        
        [FlowMethod(typeof(DamageContext))]
        public void Damage(bool forward)
        {
            if (!forward)
            {
                long dmg = GetFlowProperty<long>("Damage");
                bool crit = GetFlowProperty<bool>("Critical");

                var effects = new List<ISceneObject>();
                effects.Add(new PopupString(dmg.ToString() + (crit ? "!" : ""), crit ? ConsoleColor.Red : ConsoleColor.White, MobObj.Location, 25, crit ? 14 : 12, 0.06));


                bool showExp = GetFlowProperty<bool>("EnemyDied");
                if (showExp)
                {
                    var min = MobObj.Enemy.Level * 4;
                    var expr = RandomDungeon.Next(min, min * 2);
                    playerSceneObject.Avatar.Character.Exp(MobObj.Exp);
                    effects.Add(new PopupString($"Вы получаете {expr} опыта!", ConsoleColor.DarkMagenta, playerSceneObject.Avatar.Location, 25, 12, 0.06));
                }

                this.ShowEffects(effects);
            }
        }

        public class DamageContext
        {
            public long Damage { get; set; }

            public bool Critical { get; set; }

            public Avatar Avatar { get; set; }

            public Point Location { get; set; }

            public long Exp { get; set; }

            public bool EnemyDied { get; set; }
        }
    }
}