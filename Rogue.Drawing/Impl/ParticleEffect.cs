namespace Rogue.Drawing.Impl
{
    using Rogue.View.Interfaces;

    public class ParticleEffect : IEffect
    {
        public string Name { get; set; }

        public double Scale { get; set; }

        public string Assembly { get; set; }
    }
}