using Rogue.Abilities.Talants;
using Rogue.Classes.Noone.Talants.ElementalShield;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Noone.Talants
{
    public class ElementalShieldTalantTree : TalantTree<Noone>
    {
        public PoisonShieldTalant Posion { get; set; } = new PoisonShieldTalant() { Level = 1 };
    }
}
