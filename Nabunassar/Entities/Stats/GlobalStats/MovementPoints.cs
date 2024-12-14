namespace Nabunassar.Entities.Stats.GlobalStats
{
    internal class MovementPoints : BaseStandaloneStat
    {
        public override void BindPersona(Persona persona)
        {
            this.Value = (int)persona.PrimaryStats.Constitution;
        }
    }
}
