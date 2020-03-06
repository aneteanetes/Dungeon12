using Dungeon;
using Dungeon.Control;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12;
using System;
using System.Collections.Generic;
using System.Text;
using VegaPQ.Entities;

namespace VegaPQ.SceneObjects
{
    public class GameShard : HandleSceneControl<Cell>
    {
        public override bool Blur => true;

        private GameField gameField;

        public override bool CacheAvailable => false;

        public GameShard(Cell component, GameField gameField) : base(component, true)
        {
            this.gameField = gameField;

            this.Width = 2;
            this.Height = 2;

            this.AddTextCenter($"{component.X},{component.Y}".AsDrawText().Montserrat().InSize(20).InColor(ConsoleColor.Cyan));
        }

        private bool DestroyReady = false;
        public void AsDestory()
        {
            DestroyReady = true;
            if (!_animationAlive)
            {
                this.Destroy?.Invoke();
            }
            //this.AddTextCenter($"X".AsDrawText().Montserrat().InSize(40).InColor(ConsoleColor.Red));
        }

        public override string Image => $"shards/{(int)Component.Type}.png".AsmImgRes();

        private double originLeft;
        private double originTop;

        private Direction _direction = Direction.Idle;
        private Direction direction
        {
            get => _direction;
            set
            {
                _direction = value;

                //если idle то мы завершили движение и больше не можем двигаться
                if (value == Direction.Idle)
                {
                    CanSwitchPosition = false;
                }
            }
        }

        private bool _animationAlive = false;

        private bool AnimationAlive
        {
            get => _animationAlive;
            set
            {
                _animationAlive = value;
                if (!_animationAlive && DestroyReady)
                {
                    this.Destroy?.Invoke();
                }
            }
        }

        public bool CanSwitchPosition { get; set; }

        public void Gleam(Direction direction)
        {
            AnimationAlive = true;
            originLeft = this.Left;
            originTop = this.Top;

            this.direction = direction;
            gameField.Shard = default;
            gameField.Gleaming = true;
        }

        private double range = 0;
        private bool backward = false;

        public override bool Updatable => true;

        private TimeSpan last;

        public override void Update(GameTimeLoop gameTime)
        {
            if (!AnimationAlive)
                return;

            //если анимация только началась - устанавливаем предыдущее значение
            if (last == default)
            {
                last = gameTime.TotalGameTime;
                return;
            }

            //проверяем что прошло время на смену фрейма
            if (gameTime.TotalGameTime.TotalMilliseconds > (last.TotalMilliseconds + 3))
            {
                last = gameTime.TotalGameTime;
            }
            else return;

            // если нельзя менять позицию - делаем анимацию что нельзя
            if (!CanSwitchPosition)
            {
                // если пришли в изначальную позицию или около того
                if (range <= 0 && backward)
                {
                    gameField.Gleaming = false;
                    backward = false;
                    direction = Direction.Idle;
                    AnimationAlive = false;

                    this.Left = originLeft;
                    this.Top = originTop;
                }

                // если надо двигаться
                if (direction != Direction.Idle)
                {
                    MoveByDirection(direction, this, 0.1);
                    range += 0.1 * (backward ? (-1) : 1);
                }

                // если дошли до лимита и надо "повернуть назад"
                if (range >= 1.5)
                {
                    direction = direction.Opposite();
                    backward = true;
                }
            }
            // если можно менять позицию и мы не на месте - двигаемся
            else if (Math.Abs(this.Left - originLeft) < 2 && Math.Abs(this.Top - originTop) < 2)
            {
                MoveByDirection(direction, this, 0.1);
            }
            else
            {
                // если мы дошли до края то устанавливаем свою позицию
                gameField.Gleaming = false;
                direction = Direction.Idle;
                AnimationAlive = false;
                switch (direction)
                {
                    case Direction.Up:
                        this.Top = originTop - 2;
                        break;
                    case Direction.Down:
                        this.Top = originTop + 2;
                        break;
                    case Direction.Left:
                        this.Left = originLeft - 2;
                        break;
                    case Direction.Right:
                        this.Left = originLeft + 2;
                        break;
                    default:
                        break;
                }
            }
        }

        protected void MoveByDirection(Direction dir, ISceneObject p, double step)
        {
            switch (dir)
            {
                case Direction.Idle:
                    break;
                case Direction.Up:
                    p.Top -= step;
                    break;
                case Direction.Down:
                    p.Top += step;
                    break;
                case Direction.Left:
                    p.Left -= step;
                    break;
                case Direction.Right:
                    p.Left += step;
                    break;
                case Direction.UpLeft:
                    p.Top -= step;
                    p.Left -= step;
                    break;
                case Direction.UpRight:
                    p.Top -= step;
                    p.Left += step;
                    break;
                case Direction.DownLeft:
                    p.Top += step;
                    p.Left -= step;
                    break;
                case Direction.DownRight:
                    p.Top += step;
                    p.Left += step;
                    break;
                default:
                    break;
            }
        }

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.Click,
            ControlEventType.GlobalClickRelease
        };

        public override void Click(PointerArgs args)
        {
            if (gameField.Gleaming)
                return;

            gameField.Shard = this;
            base.Click(args);
        }

        public override void GlobalClickRelease(PointerArgs args)
        {
            gameField.Shard = default;
            gameField.MoveDirection = Direction.Idle;
            base.GlobalClickRelease(args);
        }
    }
}
