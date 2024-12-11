﻿using Nabunassar.Entities.Enums;

namespace Nabunassar.Entities.Abilities.Thief
{
    /// <summary>
    /// атака с ядом, атака дальнего боя, атака таунт ближний
    /// </summary>
    internal class ThiefAttack : Ability
    {
        public override Archetype Class => Archetype.Thief;

        public override void Bind()
        {
            Area = new AbilityArea();
            Element = Element.Physical;
            UseRange = AbilRange.Weapon;
        }

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings["Damage"]}: {Value}",
                $"{Global.Strings["Type"]}: {Element.Display()}",
                $"{Global.Strings["Range"]}: {UseRange.Display()}",
                Global.Strings["EachHandAttack"]
            };
        }
    }
}
