using Dungeon12.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Abilities.Thief
{
    /// <summary>
    /// Метка - изначально баф, потом метка молнии, сильный яд, таунт, итд
    /// </summary>
    public class ThiefMark : Ability
    {
        public override Archetype Class => Archetype.Thief;
    }
}
