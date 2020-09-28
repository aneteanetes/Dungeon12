namespace Dungeon.View.Interfaces
{
    public interface IEffect
    {
        string Name { get; }

        double Scale { get; }

        string Assembly { get; }

        public EffectTime When { get; }
    }

    public enum EffectTime
    {
        InProcess = 0,
        PostProcess = 1,
        PreProcess = 2
    }
}