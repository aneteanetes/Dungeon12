using Rogue.Transactions;
using Rogue.Types;
using Rogue.View.Interfaces;

namespace Rogue.Abilities.Talants.NotAPI
{
    public abstract class TalantDraw : Applicable, IDrawable
    {
        public string Icon { get; set; }

        public virtual string Name { get; set; }

        public IDrawColor BackgroundColor { get; set; }

        public IDrawColor ForegroundColor { get; set; }

        public virtual string Description { get; set; }

        public string Tileset => "";

        public Rectangle TileSetRegion => default;

        public Rectangle Region { get; set; }

        public bool Container => false;
    }
}
