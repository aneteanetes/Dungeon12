using System.Runtime.CompilerServices;

namespace Dungeon.View
{
    public class PossibleResolution
    {
        public PossibleResolution() { }

        public PossibleResolution(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public double CenterH(double width)
        {
            var w = (double)this.Width;
            return w / 2 - width / 2;
        }

        public double CenterV(double height)
        {
            var h = (double)this.Height;
            return h / 2 - height / 2;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PossibleResolution res))
                return false;

            return this.Width == res.Width && this.Height == res.Height;
        }

        public override int GetHashCode()
        {
            return this.Width + this.Height;
        }

        public override string ToString() => $"{Width}x{Height}";
    }
}