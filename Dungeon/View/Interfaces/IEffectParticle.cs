namespace Dungeon.View.Interfaces
{
    public interface IEffectParticle : IEffect
    {
        public bool Once { get; set; }

        public int TriggerCount { get; set; }

        public bool IsTriggered { get; set; }
    }
}