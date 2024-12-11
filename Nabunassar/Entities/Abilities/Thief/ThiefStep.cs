using Nabunassar.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nabunassar.Entities.Abilities.Thief
{
    /// <summary>
    /// перемещение по карте
    /// </summary>
    internal class ThiefStep : Ability
    {
        public override Archetype Class => Archetype.Thief;

        public override void Bind()
        {
            Cooldown = 6;
        }

        public override string[] GetTextParams()
        {
            return new string[] { };
        }
    }
}
