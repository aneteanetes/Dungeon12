using Dungeon.Control.Pointer;
using Dungeon.Types;

namespace Dungeon.Control
{
    public class PointerArgs
    {
        public int ClickCount { get; set; }
        public MouseButton MouseButton { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Dot Offset { get; set; }

        public bool Released { get; set; }

        /// <summary>
        /// Флаг указывающий что offset был вычтен
        /// </summary>
        public bool ProcessedOffset { get; set; }

        /// <summary>
        /// Координаты <see cref="X"/>, <see cref="Y"/>, учитывающие <see cref="Offset"/>
        /// </summary>
        public Dot GameCoordinates
        {
            get
            {
                var gamecoordinates = new Dot(X, Y);

                if (ProcessedOffset)
                {
                    gamecoordinates.X -= Offset.X;
                    gamecoordinates.Y -= Offset.Y;
                }

                if (Offset != default)
                {
                    if (Offset.X < 1)
                    {
                        gamecoordinates.X += Offset.X * -1;
                    }
                    else
                    {
                        gamecoordinates.X -= Offset.X;
                    }

                    if (Offset.Y < 1)
                    {
                        gamecoordinates.Y += Offset.Y * -1;
                    }
                    else
                    {
                        gamecoordinates.Y -= Offset.Y;
                    }
                }

                //gamecoordinates.X /= Settings.DrawingSize.CellF;
                //gamecoordinates.Y /= Settings.DrawingSize.CellF;

                return gamecoordinates;
            }
        }

        /// <summary>
        /// Чистые координаты <see cref="X"/>, <see cref="Y"/>
        /// </summary>
        public Dot AsPoint => new Dot(X, Y);

        /// <summary>
        /// Относительные координаты по ИГРОВОЙ СЕТКЕ (<see cref="Settings.DrawingSize.CellF"/>)
        /// </summary>
        public Dot Relative => new Dot(X / Settings.DrawingSize.CellF, Y / Settings.DrawingSize.CellF);
    }
}