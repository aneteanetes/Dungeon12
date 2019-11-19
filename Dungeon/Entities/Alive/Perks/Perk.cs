using Dungeon.Entities.Alive.Perks;
using Dungeon.Transactions;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Entities.Alive
{
    public abstract class Perk : DataEntity<Perk, DataPerk>, IDrawable
    {
        public abstract string Description { get; }

        public virtual bool ClassDependent { get; set; }
    }
}