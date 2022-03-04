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
    }
}
