namespace Dungeon12.Entities.Cooldowns
{
    internal class Cooldown
    {
        public int Value { get; set; } = 0;

        public CooldownType Type { get; set; } = CooldownType.Turn;
    }
}
