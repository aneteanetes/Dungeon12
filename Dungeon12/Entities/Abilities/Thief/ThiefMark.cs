using Dungeon12.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Abilities.Thief
{
    /// <summary>
    /// Метка - изначально баф, потом метка молнии, сильный яд, таунт, итд
    /// </summary>
    internal class ThiefMark : Ability
    {
        public override Archetype Class => Archetype.Thief;

        public override void Bind()
        {
            Area = new AbilityArea();
            Element = Element.Mental;
            Cooldown = 6;
            UseRange = AbilRange.Weapon;
        }

        public int PlusIncomingDamagePercent { get; set; } = 25;

        public int DebuffTime { get; set; } = 3;

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings["Damage"]}: {Value}",
                $"{Global.Strings["Type"]}: {Element.Display()}",
                $"{Global.Strings["Range"]}: {UseRange.Display()}",
                $"{Global.Strings["Incoming"]} {Global.Strings["Damage"].ToLowerInvariant()}: +{PlusIncomingDamagePercent}%",
                $"{Global.Strings["Active"]} {Global.Strings["Turns"].ToLowerInvariant()}: {DebuffTime}",
                Global.Strings["LeftHandAttack"],
            };
        }
    }
}