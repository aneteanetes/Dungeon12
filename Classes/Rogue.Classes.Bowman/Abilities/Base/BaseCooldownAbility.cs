using Rogue.Abilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Bowman.Abilities
{
    public abstract class BaseCooldownAbility : Ability<Bowman>
    {
        public override Cooldown Cooldown { get; } = new Cooldown(500, "Bowman");
    }
}