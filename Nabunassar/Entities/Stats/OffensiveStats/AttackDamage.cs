namespace Nabunassar.Entities.Stats.OffensiveStats
{
    internal class AttackDamage : BaseStandaloneStat
    {
        protected override string TitleSource => Global.Strings[nameof(AttackDamage)];

        public override void BindPersona(Persona persona)
        {
            Value = (int)persona.PrimaryStats.Agility;
        }
    }
}
