using System;
using System.Collections.Generic;

namespace Dungeon.Types
{
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
    }
}