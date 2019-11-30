using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dungeon.Types
{
    /// <summary>
    /// Это ссылочный тип! А значит мы получаем бесценную возможность сравнивать с null, но при этом ИЗМЕНЕНИЯ КООРДИНАТ ПРОИСХОДИТ ПО ССЫЛКЕ
    /// </summary>
    public class Point
    {
        public Point()
        {

        }

        public Point(Point fromCopy)
        {
            this.X = fromCopy.X;
            this.Y = fromCopy.Y;
        }

        public Point(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public float Xf => (float)X;

        public float Yf => (float)Y;


        const double Rad2Deg = 180.0 / Math.PI;
        const double Deg2Rad = Math.PI / 180.0;

        public double Angle(Point end)
        {
            return Math.Atan2(this.Y - end.Y, end.X - this.X);
        }

        public VectorDir VectorX { get; set; }
        public VectorDir VectorY { get; set; }

        public Point Add(double x=0,double y=0)
        {
            var p = new Point(this);
            p.X += x;
            p.Y += y;

            return p;
        }

        public Point CalculatePath(Point destination, double maxRange, double stepSize)
        {
            Point last = new Point(this);
            
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

        public override string ToString() => $"X: {X}, Y:{Y}";

        public Point Copy() => new Point(this);

        public bool EqualTo(double x, double y)
        {
            return this.X == x && this.Y == y;
        }
    }
}