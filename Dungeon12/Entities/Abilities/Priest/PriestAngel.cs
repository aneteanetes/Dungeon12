using Dungeon12.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Abilities.Priest
{
    /// <summary>
    /// Призыв ангела - по умолчанию урон, потом может лечить, наносить разный урон, постоянный суммон (инквизитор)
    /// </summary>
    public class PriestAngel : Ability
    {
        public override Archetype Class => Archetype.Priest;
    }
}
