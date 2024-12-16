using Nabunassar.Entities.Combat;

namespace Nabunassar.Entities.Abilities.Globally.Passive.Names
{
    /// <summary>
    /// Уменьшает d4 урона от X
    /// </summary>
    internal class DefenceName : GlobalAbility
    {
        public override bool IsRanked => false;

        public override DamageRange OnDamage(DamageRange damage)
        {
            damage.Decrease(new Damage(Global.DiceTower.Throw(Stats.PrimaryStats.Rank.d4), Element));
            return base.OnDamage(damage);
        }
    }
}
