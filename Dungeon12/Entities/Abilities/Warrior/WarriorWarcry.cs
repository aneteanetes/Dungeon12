﻿using Nabunassar.Entities.Enums;

namespace Nabunassar.Entities.Abilities.Warrior
{
    internal class WarriorWarcry : Ability
    {
        public override Archetype Class => Archetype.Warrior;

        public override void Bind()
        {
            Name = Global.Strings[nameof(WarriorWarcry)];
            Description = Global.Strings.Description[nameof(WarriorWarcry)];
            Area = new AbilityArea(all: true);
            Element = Element.Mental;
            Cooldown = 10;
            UseRange = AbilRange.Any;
        }

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings["Damage"]}: {Value}",
                $"{Global.Strings["Type"]}: {Element.Display()}",
                $"{Global.Strings["Range"]}: {UseRange.Display()}",
                Global.Strings["Taunt"]
            };
        }
    }
}