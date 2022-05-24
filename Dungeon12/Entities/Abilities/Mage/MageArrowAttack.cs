using Dungeon12.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Abilities.Mage
{
    internal class MageArrowAttack : Ability
    {
        public override Archetype Class => Archetype.Mage;

        public override void Bind()
        {
            Area = new AbilityArea();
            Element = Element.Magical;
            UseRange = AbilRange.Any;
        }

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings["Damage"]}: {Value}",
                $"{Global.Strings["Type"]}: {Element.Display()}",
                $"{Global.Strings["Range"]}: {UseRange.Display()}"
            };
        }
    }
}
