namespace Dungeon.Drawing.Impl
{
    using Dungeon.View.Interfaces;

    public class ParticleEffect : IEffect
    {
        public string Name { get; set; }

        public double Scale { get; set; }

        public string Assembly { get; set; }
    }
}