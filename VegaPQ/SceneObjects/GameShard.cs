using Dungeon;
using Dungeon.Control;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
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

            this.Image = $"shards/{(int)component.Type}.png".AsmImgRes();
            this.Width = 2;
            this.Height = 2;
        }

        private double originLeft;
        private double originTop;

        private Direction direction = Direction.Idle;

        public void Gleam(Direction direction)
        {
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
            if (last == default)
            {
                last = gameTime.TotalGameTime;
                return;
            }

            if (gameTime.TotalGameTime.TotalMilliseconds > (last.TotalMilliseconds + 12))
            {
                last = gameTime.TotalGameTime;
            }
            else return;

            if (range <= 0 && backward)
            {
                gameField.Gleaming = false;
                backward = false;
                direction = Direction.Idle;

                this.Left = originLeft;
                this.Top = originTop;
            }

            if (direction != Direction.Idle)
            {
                MoveByDirection(direction, this, 0.1);
                range += 0.1 * (backward ? (-1) : 1);
            }

            if (range >= 1.5)
            {
                direction = direction.Opposite();
                backward = true;
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
            base.GlobalClickRelease(args);
        }
    }
}
