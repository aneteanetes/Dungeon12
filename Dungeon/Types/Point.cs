using System;

namespace Dungeon.Types
{
    /// <summary>
    /// Это ссылочный тип! А значит мы получаем бесценную возможность сравнивать с null, но при этом ИЗМЕНЕНИЯ КООРДИНАТ ПРОИСХОДИТ ПО ССЫЛКЕ
    /// </summary>
    public class Dot
    {
        public bool IsDefault { get; private set; } = true;

        public Dot()
        {

        }

        public static Dot FromString(string xy)
        {
            if (xy == default)
                return new Dot();

            if (!xy.Contains("X:") || !xy.Contains("Y:"))
                return new Dot();

            try
            {
                var splitted = xy.Split(",", StringSplitOptions.RemoveEmptyEntries);
                double.TryParse(splitted[0].Replace("X:", ""), out var x);
                double.TryParse(splitted[1].Replace("Y:", ""), out var y);

                return new Dot(x, y);
            }
            catch
            {
                return new Dot();
            }
        }

        public Dot(Dot fromCopy)
        {
            this.X = fromCopy.X;
            this.Y = fromCopy.Y;
        }

        public Dot(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Dot(string xStrDouble, string yStrDouble)
        {
            var parsedX = double.TryParse(xStrDouble.Replace(".", ","), out var x);
            var parsedY = double.TryParse(yStrDouble.Replace(".", ","), out var y);
            if (parsedX && parsedY)
            {
                X = x;
                Y = y;
                return;
            }
            throw new ArgumentException($"{nameof(xStrDouble)} or {nameof(yStrDouble)} is not double!");
        }

        /// <summary>
        /// Static reference to zero point
        /// </summary>
        public static Dot Zero { get; } = new Dot(0, 0);

        public Dot(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool IsEvenX => X % 2 == 0;

        public bool IsEvenY => Y % 2 == 0;

        private double x { get; set; }
        public double X
        {
            get => x;
            set
            {
                x = value;
                IsDefault = false;
            }
        }

        private double y { get; set; }
        public double Y
        {
            get => y;
            set
            {
                y = value;
                IsDefault = false;
            }
        }

        public float Xf => (float)X;

        public float Yf => (float)Y;

        public int Xi => (int)X;

        public int Yi => (int)Y;

        const double Rad2Deg = 180.0 / Math.PI;
        const double Deg2Rad = Math.PI / 180.0;

        public double Angle(Dot end)
        {
            return Math.Atan2(this.Y - end.Y, end.X - this.X);
        }

        private VectorDir vectorX { get; set; }
        public VectorDir VectorX
        {
            get => vectorX;
            set
            {
                vectorX = value;
                IsDefault = false;
            }
        }

        private VectorDir vectorY { get; set; }
        public VectorDir VectorY
        {
            get => vectorY;
            set
            {
                vectorY = value;
                IsDefault = false;
            }
        }

        public Dot Add(double x=0,double y=0)
        {
            var p = new Dot(this);
            p.X += x;
            p.Y += y;

            return p;
        }

        public Dot CalculatePath(Dot destination, double maxRange, double stepSize)
        {
            Dot last = new Dot(this);
            
            var xDiff = destination.X - this.X;
            var yDiff = destination.Y - this.Y;

            VectorDir xVector = xDiff < 0 ? VectorDir.Minus : VectorDir.Plus;
            VectorDir yVector = yDiff < 0 ? VectorDir.Minus : VectorDir.Plus;

            xDiff = Math.Abs(xDiff);
            yDiff = Math.Abs(yDiff);

            if (xDiff > maxRange)
            {
                xDiff = maxRange;
            }
            if (yDiff > maxRange)
            {
                yDiff = maxRange;
            }

            var xSteps = xDiff / stepSize;
            var ySteps = yDiff / stepSize;

            var countPaths = xSteps > ySteps ? xSteps : ySteps;
            var distance = xSteps > ySteps ? xDiff : yDiff;

            var xStepSpeed = stepSize;
            var yStepSpeed = stepSize;

            if (xSteps > ySteps)
            {
                var moreDiff = ySteps / xSteps; //больше в N раз
                yStepSpeed *= moreDiff;
            }

            if (ySteps > xSteps)
            {
                var moreDiff = xSteps / ySteps; //больше в N разz
                xStepSpeed *= moreDiff;
            }

            for (double i = 0; i < distance; i += stepSize)
            {
                if (xVector == VectorDir.Plus)
                {
                    last.X += xStepSpeed;
                }
                else
                {
                    last.X -= xStepSpeed;
                }

                if (yVector == VectorDir.Plus)
                {
                    last.Y += yStepSpeed;
                }
                else
                {
                    last.Y -= yStepSpeed;
                }
            }

            return last;
        }

        public override string ToString() => $"X:{X}, Y:{Y}";

        public Dot Copy() => new Dot(this);

        public Dot Clone() => Copy();

        public bool EqualTo(double x, double y)
        {
            return this.X == x && this.Y == y;
        }

        public bool Equals(Dot point)
        {
            return this.EqualTo(point.X, point.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="another"></param>
        /// <param name="accuracy">amount of sensitivity</param>
        /// <returns></returns>
        public Direction DetectDirection(Dot another, double accuracy = 0)
        {
            Direction dirX = Direction.Idle;
            Direction dirY = Direction.Idle;

            if (Compare(true,another.x,this.x,accuracy))
            {
                dirX = Direction.Left;
            }

            if (Compare(false, another.x, this.x, accuracy))
            {
                dirX = Direction.Right;
            }

            if (Compare(false, another.y, this.y, accuracy))
            {
                dirY = Direction.Down;
            }

            if (Compare(true, another.y, this.y, accuracy))
            {
                dirY = Direction.Up;
            }

            return (Direction)((int)dirX + (int)dirY);
        }

        public Direction Move(int x = 0, int y = 0)
        {
            int dir = (int)Direction.Idle;

            if (x>0)
                dir+= (int)Direction.Right;
            if(x<0)
                dir+= (int)Direction.Left;
            if (y<0)
                dir+= (int)Direction.Up;
            if (y>0)
                dir+= (int)Direction.Down;

            this.X+=x;
            this.Y+=y;

            return (Direction)dir;
        }

        private bool Compare(bool isLess, double a, double b, double accuracy)
        {
            var diff = Math.Abs(a - b);
            if (diff < accuracy)
                return false;

            static bool less(double x1, double x2) => x1 < x2;
            static bool more(double x1, double x2) => x1 > x2;

            return isLess ? less(a, b) : more(a, b);
        }
    }
}