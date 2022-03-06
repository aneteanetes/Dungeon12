using Dungeon12.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Abilities.Mage
{
    public class MageSummon : Ability
    {
        public override Archetype Class => Archetype.Mage;

        public override void Bind()
        {
            Name = Global.Strings.MageSummon;
            Description = Global.Strings.MageSummonDescT1;
            Element = Element.Spirit;
            Cooldown = 15;
            UseRange = AbilRange.Summon;
        }

        public int SummonedHealth { get; set; } = 15;

        public int SummonedDamage { get; set; } = 10;

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings.Range}: {UseRange.Display()}",
                $"{Global.Strings.Type}: {Element.Display()}",
                " ",
                $"{Global.Strings.Health} {Global.Strings.ElemenentalUnitHis.ToLowerInvariant()}: {SummonedHealth}",
                $"{Global.Strings.Damage} {Global.Strings.ElemenentalUnitHis.ToLowerInvariant()}: {SummonedDamage}",
                $"{Global.Strings.Attack} {Global.Strings.ElemenentalUnitHis.ToLowerInvariant()}: {Element.Physical.Display()}",
            };
        }
    }
}
