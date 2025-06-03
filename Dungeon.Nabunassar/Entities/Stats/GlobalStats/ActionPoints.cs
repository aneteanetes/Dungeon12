namespace Nabunassar.Entities.Stats.GlobalStats
{
    internal class ActionPoints : BaseStandaloneStat
    {
        public override void BindPersona(Persona persona)
        {
            this.Value = (int)persona.PrimaryStats.Intelligence;
        }
    }
}
