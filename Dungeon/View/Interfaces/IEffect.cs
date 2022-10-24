using Dungeon.Types;

namespace Dungeon.View.Interfaces
{
    public interface IEffect
    {
        string Name { get; }

        double Scale { get; }

        string Assembly { get; }

        public EffectTime When { get; }

        public Dot Position { get; set; }

        public string Image { get; set; }

        public Dot Size { get; set; }
    }

    public enum EffectTime
    {
        InProcess = 0,
        PostProcess = 1,
        PreProcess = 2
    }
}