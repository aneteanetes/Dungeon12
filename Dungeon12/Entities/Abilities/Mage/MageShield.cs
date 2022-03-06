using Dungeon12.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Abilities.Mage
{
    /// <summary>
    /// щит, каменная кожа, преобразуется в фамильяра, бафы, дебафы
    /// </summary>
    public class MageShield : Ability
    {
        public override Archetype Class => Archetype.Mage;

        public override void Bind()
        {
            Name = Global.Strings.MageShield;
            Description = Global.Strings.MageShieldDescT1;
            Area = new AbilityArea(friendlytarget: true);
            Element = Element.Magical;
            Cooldown = 8;
            UseRange = AbilRange.Friendly;
        }

        public int BarrierValue { get; set; }

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings.Defence}: +{Value}",
                $"{Global.Strings.Barrier}: +{BarrierValue}",
                $"{Global.Strings.Type}: {Element.Display()}",
                $"{Global.Strings.Range}: {UseRange.Display()}"
            };
        }
    }
}
