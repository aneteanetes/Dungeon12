using Rogue.Transactions;
using Rogue.Types;
using Rogue.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Entites.Alive
{
    public abstract class Perk : Applicable, IDrawable
    {
        public abstract string Icon { get; }

        public abstract string Name { get; }

        public abstract IDrawColor BackgroundColor { get; set; }

        public abstract IDrawColor ForegroundColor { get; set; }

        public abstract string Description { get; }

        public string Tileset => "";

        public Rectangle TileSetRegion => default;

        public Rectangle Region { get; set; }

        public bool Container => false;

        public virtual bool ClassDependent { get; set; }

        public string Uid { get; } = Guid.NewGuid().ToString();
    }

}
