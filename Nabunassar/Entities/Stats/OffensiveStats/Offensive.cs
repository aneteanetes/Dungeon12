namespace Nabunassar.Entities.Stats.OffensiveStats
{
    internal class Offensive
    {
        public AttackDamage AttackDamage { get; set; } = new();

        public AbilityPower AbilityPower { get; set; } = new();

        public Armor Armor { get; set; } = new();

        public Barrier Barrier { get; set; } = new();

        public void BindPersona(Persona persona)
        {
            AttackDamage.BindPersona(persona);
            AbilityPower.BindPersona(persona);
            Armor.BindPersona(persona);
            Barrier.BindPersona(persona);
        }
    }
}
