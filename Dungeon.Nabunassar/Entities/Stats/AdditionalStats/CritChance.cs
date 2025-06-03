namespace Nabunassar.Entities.Stats.AdditionalStats
{
    internal class CritChance : BaseStandaloneStat
    {
        public override void BindPersona(Persona persona)
        {
            Value = ((int)persona.PrimaryStats.Agility) * 5;
        }
    }
}
