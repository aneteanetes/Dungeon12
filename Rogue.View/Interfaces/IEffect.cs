namespace Rogue.View.Interfaces
{
    public interface IEffect
    {
        string Name { get; set; }

        double Scale { get; set; }

        string Assembly { get; }
    }
}
