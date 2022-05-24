using Dungeon12.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Abilities.Thief
{
    /// <summary>
    /// магическая ближняя атака - преобразуется в полноценную магию тьмы
    /// </summary>
    internal class ThiefShadow : Ability
    {
        public override Archetype Class => Archetype.Thief;

        public override void Bind()
        {
            Area = new AbilityArea(true, true);
            Element = Element.DarkMagic;
            Cooldown = 2;
            UseRange = AbilRange.Weapon;
        }

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings["Damage"]}: {Value}",
                $"{Global.Strings["Type"]}: {Element.Display()}",
                $"{Global.Strings["Range"]}: {UseRange.Display()}",
                Global.Strings["RightHandAttack"]
            };
        }
    }
}
