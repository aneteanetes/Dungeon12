using Nabunassar.Entities.Combat;

namespace Nabunassar.Entities.Abilities.Battle.Archetyped.Warriors
{
    internal class WarriorAttack : BattleAbility
    {
        public override DamageRange OnAttack(DamageRange damage)
        {
            damage.Add(new Damage(Global.DiceTower.Throw(Rank), Enums.Element.Physical)
            {
                IsIgnoreDefence = true,
            });

            return base.OnAttack(damage);
        }
    }
}