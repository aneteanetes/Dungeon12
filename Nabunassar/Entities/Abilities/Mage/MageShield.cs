using Nabunassar.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nabunassar.Entities.Abilities.Mage
{
    /// <summary>
    /// щит, каменная кожа, преобразуется в фамильяра, бафы, дебафы
    /// </summary>
    internal class MageShield : Ability
    {
        public override Archetype Archetype => Archetype.Mage;

        public override void Bind()
        {
            Area = new AbilityArea(friendlytarget: true);
            Element = Element.Magical;
            Cooldown = 8;
            UseRange = AbilRange.Friendly;
        }

        public int BarrierValue { get; set; }

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings["Defence"]}: +{Value}",
                $"{Global.Strings["Barrier"]}: +{BarrierValue}",
                $"{Global.Strings["Type"]}: {Element.Display()}",
                $"{Global.Strings["Range"]}: {UseRange.Display()}"
            };
        }
    }
}
