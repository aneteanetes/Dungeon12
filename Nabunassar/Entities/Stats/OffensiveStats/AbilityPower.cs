namespace Nabunassar.Entities.Stats.OffensiveStats
{
    internal class AbilityPower : BaseStandaloneStat
    {
        public override void BindPersona(Persona persona)
        {
            Value = (int)persona.PrimaryStats.Intelligence;
        }
    }
}
