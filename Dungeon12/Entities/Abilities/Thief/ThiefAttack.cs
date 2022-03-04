using Dungeon12.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Abilities.Thief
{
    /// <summary>
    /// атака с ядом, атака дальнего боя, атака таунт ближний
    /// </summary>
    public class ThiefAttack : Ability
    {
        public override Archetype Class => Archetype.Thief;
    }
}
