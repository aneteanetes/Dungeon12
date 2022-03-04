using Dungeon12.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Abilities.Mage
{
    /// <summary>
    /// щит, каменная кожа, преобразуется в фамильяра, бафы, дебафы
    /// </summary>
    public class MageShield : Ability
    {
        public override Archetype Class => Archetype.Mage;
    }
}
