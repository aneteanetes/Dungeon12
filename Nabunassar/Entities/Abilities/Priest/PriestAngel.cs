﻿using Nabunassar.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nabunassar.Entities.Abilities.Priest
{
    /// <summary>
    /// Призыв ангела - по умолчанию урон, потом может лечить, наносить разный урон, постоянный суммон (инквизитор)
    /// </summary>
    internal class PriestAngel : Ability
    {
        public override Archetype Class => Archetype.Priest;

        public override void Bind()
        {
            Element = Element.HolyMagic;
            Cooldown = 6;
            UseRange = AbilRange.Summon;
        }

        public int HealEffective { get; set; } = 30;

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings["Range"]}: {UseRange.Display()}",
                $"{Global.Strings["Type"]}: {Element.Display()}",
                " ",
                $"{Global.Strings["HealEffective"]}: +{HealEffective}%",
                $"{Global.Strings["Sacrifice"]}",
                $"{Global.Strings["Active"]} {Global.Strings["Turns"].ToString().ToLowerInvariant()}: 3",
            };
        }
    }
}
