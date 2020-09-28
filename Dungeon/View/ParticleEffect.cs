namespace Dungeon.Drawing.Impl
{
    using Dungeon.View.Interfaces;

    public sealed class ParticleEffect : IEffect
    {
        public ParticleEffect()
        {
            Assembly = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
        }

        public string Name { get; set; }

        public double Scale { get; set; }

        public string Assembly { get; }

        public EffectTime When => EffectTime.InProcess;
    }
}