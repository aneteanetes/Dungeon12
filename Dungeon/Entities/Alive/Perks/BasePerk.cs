using Dungeon.Transactions;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Entites.Alive
{
    public abstract class BasePerk : Perk
    {
        public override string Description { get; }
    }

}
