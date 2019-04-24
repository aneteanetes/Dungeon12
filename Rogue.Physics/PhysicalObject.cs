using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.Physics
{
    public class PhysicalObject
    {
        public virtual PhysicalSize Size { get; set; }

        public virtual PhysicalPosition Position { get; set; }

        public virtual PhysicalObject Vision { get; set; }

        public bool Contains(PhysicalObject one)
        {
            var xContains = one.Position.X >= this.Position.X
                && one.Position.X < this.Size.Width;

            var yCOntains = one.Position.Y >= this.Position.Y
                && one.Position.Y < this.Size.Height;

            return xContains && yCOntains;
        }

        public bool IntersectsWith(PhysicalObject @object) => Intersect(this, @object);

        public static bool Intersect(PhysicalObject a, PhysicalObject b)
        {
            var x1 = Math.Max(a.Position.X, b.Position.X);
            var x2 = Math.Min(a.Position.X  + a.Size.Width, b.Position.X  + b.Size.Width);
            var y1 = Math.Max(a.Position.Y , b.Position.Y );
            var y2 = Math.Min(a.Position.Y  + a.Size.Height, b.Position.Y  + b.Size.Height);

            if (x2 >= x1 && y2 >= y1)
            {
                return true;
            }

            return false;
        }

        public IEnumerable<T> InVision<T>(IEnumerable<T> available) where T : PhysicalObject
            => available.Where(physObj => this.IntersectsWith(physObj));
    }
}