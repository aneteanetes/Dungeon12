namespace Dungeon.Physics
{
    public class PhysicalSize
    {
        public PhysicalSize() { }

        public PhysicalSize(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

        public virtual double Height { get; set; }

        public virtual double Width { get; set; }

        public PhysicalSize Copy() => new PhysicalSize()
        {
            Height = this.Height,
            Width = this.Width
        };
    }
}