using Rogue.Transactions;
using Rogue.Types;
using Rogue.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Entites.Alive
{
    public abstract class BasePerk : Perk
    {
        public override string Icon { get; }

        public override string Name { get; }

        public override IDrawColor BackgroundColor { get; set; }
        public override IDrawColor ForegroundColor { get; set; }

        public override string Description { get; }
    }

}
