using Dungeon12.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Abilities.Thief
{
    /// <summary>
    /// магическая ближняя атака - преобразуется в полноценную магию тьмы
    /// </summary>
    public class ThiefShadow : Ability
    {
        public override Archetype Class => Archetype.Thief;
    }
}
