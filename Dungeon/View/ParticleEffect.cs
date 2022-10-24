namespace Dungeon.Drawing.Impl
{
    using Dungeon.Types;
    using Dungeon.View.Interfaces;

    public sealed class ParticleEffect : IEffectParticle
    {
        public ParticleEffect()
        {
            Assembly = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
        }

        public string Name { get; set; }

        public double Scale { get; set; }

        public string Assembly { get; }

        public EffectTime When => EffectTime.InProcess;

        public Dot Position { get; set; }

        public string Image { get; set; }

        public Dot Size { get; set; }

        public bool Once { get; set; }

        public bool IsTriggered { get; set; }
        public int TriggerCount { get; set; }
    }
}