using Dungeon;
using Dungeon.Control;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        public override string Image => $"shards/{(int)Component.Type}.png".AsmImgRes();

        private ShardAnimationSettings animationSettings;

        private TaskCompletionSource<bool> gleamSource;

        public Task<bool> GleamAsync(Direction dir, bool back = true)
        {
            gleamSource = new TaskCompletionSource<bool>();

            animationSettings = new ShardAnimationSettings()
            {
                OriginLeft = this.Left,
                OriginTop = this.Top,
                Direction = dir,
                Backward = back,
                MaxRange = back
                ? 1.5
                : 2
            };

            return gleamSource.Task;
        }

        public override bool Updatable => true;
        
        public override void Update(GameTimeLoop gameTime)
        {
            //если не анимируемся
            if (animationSettings==default)
                return;

            //если анимация только началась - устанавливаем предыдущее значение
            if (animationSettings.LastUpdate == default)
            {
                animationSettings.LastUpdate = gameTime.TotalGameTime;
                return;
            }

            //проверяем что прошло время на смену фрейма
            if (gameTime.TotalGameTime.TotalMilliseconds > (animationSettings.LastUpdate.TotalMilliseconds + 3))
            {
                animationSettings.LastUpdate = gameTime.TotalGameTime;
            }
            else return;

            // если не можем изменить позицию и надо "отскочить"
            if (animationSettings.Backward)
            {
                // если пришли в изначальную позицию после того как отскакивали назад
                if (animationSettings.Range <= 0 && animationSettings.BackwardPlayed)
                {
                    // рисуемся в оригинальном месте
                    this.Left = animationSettings.OriginLeft;
                    this.Top = animationSettings.OriginTop;

                    // уничтожаем анимацию
                    animationSettings = default;

                    //await'имся
                    gleamSource.SetResult(true);
                    return;
                }

                // если дошли до лимита и надо "повернуть назад"
                if (animationSettings.Range >= animationSettings.MaxRange && !animationSettings.BackwardPlayed)
                {
                    animationSettings.BackwardPlayed = true;
                    animationSettings.Direction = animationSettings.Direction.Opposite();
                }
                else // если надо двигаться
                {
                    MoveByDirection(animationSettings.Direction, this, 0.1);
                    animationSettings.Range += 0.1 * (animationSettings.BackwardPlayed ? (-1) : 1);
                }
            }
            // если можно менять позицию и мы не на месте - двигаемся
            else if (Math.Abs(this.Left - animationSettings.OriginLeft) < 2 && Math.Abs(this.Top - animationSettings.OriginTop) < 2)
            {
                MoveByDirection(animationSettings.Direction, this, 0.1);
            }
            else
            {
                // если мы дошли до края то устанавливаем свою позицию
                switch (animationSettings.Direction)
                {
                    case Direction.Up:
                        this.Top = animationSettings.OriginTop - animationSettings.MaxRange;
                        break;
                    case Direction.Down:
                        this.Top = animationSettings.OriginTop + animationSettings.MaxRange;
                        break;
                    case Direction.Left:
                        this.Left = animationSettings.OriginLeft - animationSettings.MaxRange;
                        break;
                    case Direction.Right:
                        this.Left = animationSettings.OriginLeft + animationSettings.MaxRange;
                        break;
                    default:
                        break;
                }

                // уничтожаем анимацию
                animationSettings = default;

                //await'имся
                gleamSource.SetResult(true);
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
            gameField.Shard = this;
            base.Click(args);
        }

        public override void GlobalClickRelease(PointerArgs args)
        {
            if(gameField.Shard==this)
            {
                gameField.Shard = default;
            }
            base.GlobalClickRelease(args);
        }

        private class ShardAnimationSettings
        {
            public double MaxRange { get; set; }

            public double Range { get; set; }

            public bool Backward { get; set; }

            public Direction Direction { get; set; }

            public double OriginLeft { get; set; }

            public double OriginTop { get; set; }

            public TimeSpan LastUpdate { get; set; }

            public bool BackwardPlayed { get; set; }
        }
    }    
}
