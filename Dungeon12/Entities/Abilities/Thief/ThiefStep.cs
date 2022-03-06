using Dungeon12.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Abilities.Thief
{
    /// <summary>
    /// перемещение по карте
    /// </summary>
    public class ThiefStep : Ability
    {
        public override Archetype Class => Archetype.Thief;

        public override void Bind()
        {
            Name = Global.Strings.ThiefStep;
            Description = Global.Strings.ThiefStepDescT1;
            Cooldown = 6;
        }

        public override string[] GetTextParams()
        {
            return new string[] { };
        }
    }
}
