using Nabunassar.Entities.Characters;

namespace Nabunassar.Entities.Abilities.Globally.Passive.Names
{
    /// <summary>
    /// Добавляет d4 силы магии при использовании способностей
    /// </summary>
    internal class MagictableName : GlobalAbility
    {
        public MagictableName()
        {
            Rank = Stats.PrimaryStats.Rank.d4;
        }

        public override bool IsRanked => false;

        public override bool IsAPBooser => true;

        public override bool IsApplicable(Ability ability)
        {
            return ability.Element == Persona.Race.Element();
        }
    }
}
