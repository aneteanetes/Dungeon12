using Nabunassar.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nabunassar.Entities.Abilities.Mage
{
    internal class MageSummon : Ability
    {
        public override Archetype Archetype => Archetype.Mage;

        public override void Bind()
        {
            Element = Element.Spirit;
            Cooldown = 15;
            UseRange = AbilRange.Summon;
        }

        public int SummonedHealth { get; set; } = 15;

        public int SummonedDamage { get; set; } = 10;

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings["Range"]}: {UseRange.Display()}",
                $"{Global.Strings["Type"]}: {Element.Display()}",
                " ",
                $"{Global.Strings["Health"]} {Global.Strings["ElemenentalUnitHis"].ToString().ToLowerInvariant()}: {SummonedHealth}",
                $"{Global.Strings["Damage"]} {Global.Strings["ElemenentalUnitHis"].ToString().ToLowerInvariant()}: {SummonedDamage}",
                $"{Global.Strings["Attack"]} {Global.Strings["ElemenentalUnitHis"].ToString().ToLowerInvariant()}: {Element.Physical.Display()}",
            };
        }
    }
}
