namespace Nabunassar.Entities.Stats
{
    internal class Health : BaseRangeStat
    {
        public  override void BindPersona(Persona persona)
        {
            BaseValue = persona.PrimaryStats.CalculateBaseHealth();
            NowValue = BaseValue;
            MaxValue = NowValue;
        }
    }
}
