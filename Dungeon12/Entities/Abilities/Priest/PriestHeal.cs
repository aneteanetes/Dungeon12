using Dungeon12.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Abilities.Priest
{
    /// <summary>
    /// Исцеление, преобразуется в щиты, сильное исцеление, исцеление по времени
    /// </summary>
    public class PriestHeal : Ability
    {
        public override Archetype Class => Archetype.Priest;
    }
}
