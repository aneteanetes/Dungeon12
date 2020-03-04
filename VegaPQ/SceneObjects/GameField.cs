using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Types;
using System;
using System.Collections.Generic;
using System.Text;
using VegaPQ.Entities;

namespace VegaPQ.SceneObjects
{
    public class GameField : DarkRectangle
    {
        public override bool CacheAvailable => false;

        public override bool Updatable => true;

        public GameField()
        {
            this.Width = 14;
            this.Height = 14;
            this.Opacity = 0.7;

            field = new GameShard[7, 7];

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    var cell = new Cell() { Type = (CellType)RandomDungeon.Range(1, 5), X = i, Y = j };
                    var shard = new GameShard(cell, this)
                    {
                        Left = i * 2,
                        Top = j * 2
                    };
                    this.AddChild(shard);
                    field[i, j] = shard;
                }
            }
        }

        private readonly GameShard[,] field;

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.GlobalMouseMove
        };

        public Direction MoveDirection { get; private set; }

        private GameShard _currentShard;
        public GameShard Shard
        {
            get => _currentShard;
            set
            {
                if (_currentShard != value)
                {
                    prev = default;
                }
                _currentShard = value;
            }
        }

        public bool Gleaming { get; set; }

        PointerArgs prev;
        public override void GlobalMouseMove(PointerArgs args)
        {
            if (Gleaming || Shard==default)
                return;

            if (prev == default)
            {
                prev = args;
                return;
            }

            if (Math.Abs(args.X - prev.X) < .10 && Math.Abs(args.Y - prev.Y) < .10)
                return;

            Direction moveDirection = MoveDirection;

            var x = Math.Abs(args.X - prev.X);
            var y = Math.Abs(args.Y - prev.Y);

            if (x > y)
            {
                MoveDirection = args.X > prev.X
                    ? Direction.Right
                    : Direction.Left;
            }
            else if (y > x)
            {
                MoveDirection = args.Y > prev.Y
                    ? Direction.Down
                    : Direction.Up;
            }

            if (MoveDirection != moveDirection)
            {
                var component = Shard.Component;
                Shard.Gleam(MoveDirection);
                switch (MoveDirection)
                {
                    case Direction.Up:
                        field[component.X, component.Y - 1].Gleam(MoveDirection.Opposite());
                        break;
                    case Direction.Down:
                        field[component.X, component.Y + 1].Gleam(MoveDirection.Opposite());
                        break;
                    case Direction.Left:
                        field[component.X-1, component.Y].Gleam(MoveDirection.Opposite());
                        break;
                    case Direction.Right:
                        field[component.X + 1, component.Y].Gleam(MoveDirection.Opposite());
                        break;
                    default:
                        break;
                }
            }

            prev = args;
        }

        private void RequestChange()
        { 
            // проверяем могут ли камни переместиться
            // передаём в камни информацию о том что они "могут" или "не могут"
            // для того что бы проиграть различную анимацию (разная дистанция)
        }

        private void CalculateChanges()
        {
            // производим все вычисления:
            // удаляем совпадающие камни
            // заполняем сзади цвет
            // кароч вся игровая логика
        }

        private void FillField()
        {
            // если мы переместили таки камни, теперь надо заполнить поле
            // находим пустоты
            // удаляем старые
            // сверху кидаем новые камни
            // запускаем адовую анимацию сверху вниз
        }
    }
}