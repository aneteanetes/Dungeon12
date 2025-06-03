namespace Nabunassar.Entities.Stats.AdditionalStats
{
    internal class SpellReflection : BaseStandaloneStat
    {
        public override void BindPersona(Persona persona)
        {
            Value = ((int)persona.PrimaryStats.Intelligence) * 5;
        }
    }
}
