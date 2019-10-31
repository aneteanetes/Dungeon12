using Dungeon.Types;

namespace Dungeon.Control.Pointer
{
    public class PointerArgs
    {
        public int ClickCount { get; set; }
        public MouseButton MouseButton { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Point Offset { get; set; }

        /// <summary>
        /// Флаг указывающий что offset был вычтен
        /// </summary>
        public bool ProcessedOffset { get; set; }

        public Point GameCoordinates
        {
            get
            {
                var gamecoordinates = new Point(X,Y);

                if (ProcessedOffset)
                {
                    gamecoordinates.X -= Offset.X;
                    gamecoordinates.Y -= Offset.Y;
                }

                if (Offset != default)
                {
                    if (Offset.X < 1)
                    {
                        gamecoordinates.X += (Offset.X * -1);
                    }
                    else
                    {
                        gamecoordinates.X -= (Offset.X);
                    }

                    if (Offset.Y < 1)
                    {
                        gamecoordinates.Y += (Offset.Y * -1);
                    }
                    else
                    {
                        gamecoordinates.Y -= (Offset.Y);
                    }
                }

                gamecoordinates.X /= 32;
                gamecoordinates.Y /= 32;

                return gamecoordinates;
            }
        }

        public Point AsPoint => new Point(X, Y);

        /// <summary>
        /// Относительные координаты по игровой сетке
        /// </summary>
        public Point Relative => new Point(X / 32, Y / 32);
    }
}