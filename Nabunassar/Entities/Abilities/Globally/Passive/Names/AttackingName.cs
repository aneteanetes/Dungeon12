using Nabunassar.Entities.Combat;

namespace Nabunassar.Entities.Abilities.Globally.Passive.Names
{
    /// <summary>
    /// Добавляет d4 чистого урона от X
    /// </summary>
    internal class AttackingName : GlobalAbility
    {
        public override bool IsRanked => false;

        public override bool IsAvailableForCutting => false;

        public override DamageRange OnAttack(DamageRange damage)
        {
            damage.Add(new Damage(Global.DiceTower.Throw(Stats.PrimaryStats.Rank.d4), Element));
            return base.OnAttack(damage);
        }
    }
}
