namespace Rogue.View.Interfaces
{
    public interface IDrawColor
    {
        byte R { get; }
        byte G { get; }
        byte B { get; }
        byte A { get; }
        double Opacity { get; }
    }
}