using Dungeon.Transactions;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Entites.Alive
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
