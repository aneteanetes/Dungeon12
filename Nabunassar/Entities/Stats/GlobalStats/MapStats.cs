namespace Nabunassar.Entities.Stats.GlobalStats
{
    internal class MapStats : BaseStat
    {
        public MovementPoints MovementPoints { get; set; } = new();

        public ActionPoints ActionPoints { get; set; } = new();

        public override void BindPersona(Persona persona)
        {
            MovementPoints.BindPersona(persona);
            ActionPoints.BindPersona(persona);
        }
    }
}