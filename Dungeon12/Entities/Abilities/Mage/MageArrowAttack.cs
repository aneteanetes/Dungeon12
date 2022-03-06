using Dungeon12.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Abilities.Mage
{
    public class MageArrowAttack : Ability
    {
        public override Archetype Class => Archetype.Mage;

        public override void Bind()
        {
            Name = Global.Strings.MageArrowAttack;
            Description = Global.Strings.MageArrowAttackDescT1;
            Area = new AbilityArea();
            Element = Element.Magical;
            UseRange = AbilRange.Any;
        }

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings.Damage}: {Value}",
                $"{Global.Strings.Type}: {Element.Display()}",
                $"{Global.Strings.Range}: {UseRange.Display()}"
            };
        }
    }
}
