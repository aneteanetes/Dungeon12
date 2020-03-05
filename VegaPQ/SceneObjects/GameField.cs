using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Types;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VegaPQ.Entities;

namespace VegaPQ.SceneObjects
{
    public class GameField : DarkRectangle
    {
        public override bool CacheAvailable => false;

        public override bool Updatable => true;

        private int sizeX = 0;
        private int sizeY = 0;

        public GameField(int width = 0, int height = 0)
        {
            sizeX = width;
            sizeY = height;

            this.Width = width * 2;
            this.Height = height * 2;
            this.Opacity = 0.7;

            GenerateField();
            while (Shuffle() != 0) ;
        }

        private void GenerateField()
        {
            this.ClearChildrens();

            var cellsTypes = typeof(CellType).All<CellType>().Count();

            field = new GameShard[sizeX, sizeY];

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    var cell = new Cell() { Type = (CellType)RandomDungeon.Range(1, cellsTypes), X = x, Y = y };
                    var shard = new GameShard(cell, this)
                    {
                        Left = x * 2,
                        Top = y * 2
                    };
                    this.AddChild(shard);
                    field[x, y] = shard;
                }
            }
        }

        private int Shuffle()
        {
            var sum = 0;

            for (int y = 0; y < sizeX; y++)
            {
                for (int x = 0; x < sizeY; x++)
                {
                    var invalidation = InvalidateField(field[x, y].Component);
                    var count = invalidation.Count();
                    if (count > 0)
                    {
                        sum++;
                        foreach (var invalid in invalidation)
                        {
                            var intType = (int)invalid.Component.Type;
                            intType += RandomDungeon.Range(1,3);
                            if (intType > 5)
                            {
                                intType -= 5;
                            }

                            invalid.Component.Type = (CellType)intType;
                        }
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                }
            }

            return sum;
        }

        private GameShard[,] field;

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.GlobalMouseMove,
            ControlEventType.Key
        };

        protected override Key[] KeyHandles => new Key[]
        {
            Key.F5, Key.F6
        };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.F5)
                GenerateField();

            if (key == Key.F6) 
                while (Shuffle() != 0) ;
        }

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
                var target = GetTargetShard(Shard.Component, MoveDirection);

                if (target == default)
                {
                    this.Shard = default;
                    return;
                }

                RequestChange(Shard, target, MoveDirection);
                Shard.Gleam(MoveDirection);
                target.Gleam(MoveDirection.Opposite());
            }

            prev = args;
        }

        private GameShard GetTargetShard(Cell component, Direction direction)
        {
            var x = component.X;
            var y = component.Y;

            switch (MoveDirection)
            {
                case Direction.Up:
                    if (y == 0)
                        return default;
                    return field[x, y - 1];

                case Direction.Down:
                    if (y == 6)
                        return default;
                    return field[x, y + 1];

                case Direction.Left:
                    if (x == 0)
                        return default;
                    return field[x - 1, y];

                case Direction.Right:
                    if (x == 6)
                        return default;
                    return field[x + 1, y];

                default: return default;
            }
        }

        /// <summary>
        /// Получаем все камни которые совпадают начиная от поиска
        /// </summary>
        /// <param name="fromPoint">Место откуда мы начинаем искать совпадения рядом</param>
        private IEnumerable<GameShard> InvalidateField(Cell fromPoint)
        {
            if (fromPoint == default)
                return Enumerable.Empty<GameShard>();

            var x = fromPoint.X;
            var y = fromPoint.Y;
            var searchType = fromPoint.Type;

            var nearest = new List<GameShard>();
            var vertical = new List<GameShard>();

            //check bounds
            if(x<0 || x>=sizeX || y<0 || y>=sizeY)
                return Enumerable.Empty<GameShard>();

            HorizontalSearch(x, y, searchType, nearest, vertical);
            
            if (nearest.Distinct().Count() < 3)
            {
                nearest = new List<GameShard>();
            }
            if (vertical.Distinct().Count() < 3)
            {
                vertical = new List<GameShard>();
            }

            return nearest.Concat(vertical).Distinct();
        }

        private void HorizontalSearch(int x, int y, CellType searchType, List<GameShard> nearest, List<GameShard> verical)
        {
            //ищем вправо 
            for (int ix = x; ix < sizeX && x >= 0; ix++)
            {
                if (GetShard(ix, y, searchType, out var next))
                {
                    nearest.Add(next);
                    VerticalSearch(y, searchType, verical, ix);
                }
                else break;
            }

            for (int ix = x; ix >= 0 && x < sizeX; ix--)
            {
                if (GetShard(ix, y, searchType, out var next))
                {
                    nearest.Add(next);
                    VerticalSearch(y, searchType, verical, ix);
                }
                else break;
            }
        }

        private void VerticalSearch(int y, CellType searchType, List<GameShard> verical, int i)
        {
            //ищем наверх
            for (int iy = y; iy < sizeY && iy>=0; iy++)
            {
                if (GetShard(i, iy, searchType, out var nextY))
                {
                    verical.Add(nextY);
                }
                else break;
            }

            //ищем вниз
            for (int iy = y; iy >= 0 && iy < sizeY; iy--)
            {
                if (GetShard(i, iy, searchType, out var nextY))
                {
                    verical.Add(nextY);
                }
                else break;
            }
        }

        private bool GetShard(int x, int y, CellType type, out GameShard next)
        {
            try
            {
                next = field[x, y];
            }
            catch (IndexOutOfRangeException)
            {
                throw;
            }

            //это потому что сейчас поле непересчитывается
            if (next.Component == default)
                return false;

            return next.Component.Type == type;
        }

        private bool RequestChange(GameShard moved, GameShard target, Direction dir)
        {
            // проверяем могут ли камни переместиться
            // передаём в камни информацию о том что они "могут" или "не могут"
            // для того что бы проиграть различную анимацию (разная дистанция)
            SwitchCoords(moved, target);

            var fromTarget = InvalidateField(target.Component);
            var fromMoved = InvalidateField(moved.Component);

            var targetMatch = fromTarget.Count() > 0;
            var movedMatch = fromMoved.Count() > 0;

            if (targetMatch || movedMatch)
            {
                if (targetMatch)
                {
                    target.CanMatch = true;
                }
                if (movedMatch)
                {
                    moved.CanMatch = true;
                }
                target.CanSwitchPosition = true;
                moved.CanSwitchPosition = true;

                CalculateChanges();
            }
            else
            {
                SwitchCoords(target, moved);
            }

            return default;
        }

        private void SwitchCoords(GameShard moved, GameShard target)
        {
            field[target.Component.X, target.Component.Y] = moved;
            field[moved.Component.X, moved.Component.Y] = target;

            var fromX = moved.Component.X;
            var fromY = moved.Component.Y;

            moved.Component.X = target.Component.X;
            moved.Component.Y = target.Component.Y;

            target.Component.Y = fromY;
            target.Component.X = fromX;
        }

        private void CalculateChanges()
        {
            List<GameShard> fordelete = new List<GameShard>();
            
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    var invalidation = InvalidateField(field[x, y].Component);
                    var count = invalidation.Count();
                    if (count > 0)
                    {
                        fordelete.AddRange(invalidation);
                    }
                }
            }

            foreach (var delet in fordelete)
            {
                //delet.Destroy?.Invoke();
                delet.AsDestory();
            }

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