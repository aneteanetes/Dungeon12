﻿using Nabunassar.Entities.Enums;

namespace Nabunassar.Entities.Abilities.Warrior
{
    internal class WarriorAttack : Ability
    {
        public override Archetype Class => Archetype.Warrior;

        public override void Bind()
        {
            Area = new AbilityArea();
            Element = Element.Physical;
            UseRange = AbilRange.Close;
        }

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings["Damage"]}: {Value}",
                $"{Global.Strings["Type"]}: {Element.Display()}",
                $"{Global.Strings["Range"]}: {UseRange.Display()}",
                Global.Strings["IgnoreTargetPhysicalDefence"]
            };
        }
    }
}
