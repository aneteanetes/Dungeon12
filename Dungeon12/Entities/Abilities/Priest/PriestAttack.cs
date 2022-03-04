using Dungeon12.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Abilities.Priest
{
    /// <summary>
    /// Атака оружием, преобразуется в исцеляющую атаку, таунт, оглушение
    /// </summary>
    public class PriestAttack : Ability
    {
        public override Archetype Class => Archetype.Priest;
    }
}
