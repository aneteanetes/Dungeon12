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