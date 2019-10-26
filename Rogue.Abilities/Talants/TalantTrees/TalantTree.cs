using Rogue.Abilities.Talants.NotAPI;
using Rogue.Types;
using Rogue.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.Abilities.Talants.TalantTrees
{

    public abstract class TalantTree : IDrawable
    {
        public virtual string Name { get; set; }

        public string Icon { get; }

        public abstract string Tileset { get; }

        public Rectangle TileSetRegion { get; }

        public bool Container => false;

        public IDrawColor BackgroundColor { get; set; }

        public IDrawColor ForegroundColor { get; set; }

        public Rectangle Region { get; set; }

        public abstract List<IGrouping<int,TalantBase>> Talants { get; }

        public string Uid { get; } = Guid.NewGuid().ToString();
    }
}
