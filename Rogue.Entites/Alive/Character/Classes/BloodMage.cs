using Rogue.Entites.Abilities;
using Rogue.Entites.Alive.Character.Attributes;
using Rogue.Entites.Enums;

namespace Rogue.Entites.Alive.Character.Classes
{
    [Class("Маг крови")]
    public class BloodMage : Player
    {
        [Ability(1, "Вампиризм", Scale.AbilityPower, 0.1)]
        public void Vampirism(Ability ability)
        {

        }

        [Ability(2, "Кровавое копьё", Scale.AbilityPower, 0.1)]
        public void BloodSpear(Ability ability)
        {

        }

        [Ability(3, "Щит крови", Scale.AbilityPower, 0.1)]
        public void BloodShield(Ability ability)
        {

        }

        [Ability(4, "Форма гуля", Scale.AbilityPower, 0.1)]
        public void Ghoul(Ability ability)
        {

        }
    }
}