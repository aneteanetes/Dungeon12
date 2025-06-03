using Nabunassar.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nabunassar.Entities.Abilities.Priest
{
    /// <summary>
    /// Исцеление, преобразуется в щиты, сильное исцеление, исцеление по времени
    /// </summary>
    internal class PriestHeal : Ability
    {
        public override Archetype Archetype => Archetype.Priest;

        public override void Bind()
        {
            Area = new AbilityArea(friendlytarget: true);
            Element = Element.Holy;
            UseRange = AbilRange.Friendly;
        }

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings["Heal"]}: {Value}",
                $"{Global.Strings["Type"]}: {Element.Display()}",
                $"{Global.Strings["Range"]}: {UseRange.Display()}"
            };
        }
    }
}
