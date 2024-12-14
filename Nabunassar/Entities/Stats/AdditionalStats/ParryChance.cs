namespace Nabunassar.Entities.Stats.AdditionalStats
{
    /// <summary>
    /// Характеристика специализации
    /// </summary>
    internal class ParryChance : BaseStandaloneStat
    {
        public override void BindPersona(Persona persona)
        {
            Value = 0;
        }
    }
}
