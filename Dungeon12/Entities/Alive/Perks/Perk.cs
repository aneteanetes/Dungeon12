using Dungeon.View.Interfaces;
using Dungeon12.Data.Perks;

namespace Dungeon12.Entities.Alive
{
    public abstract class Perk : DataEntityFraction<Perk, ValuePerk>, IDrawable
    {
        public abstract string Description { get; }

        public virtual bool ClassDependent { get; set; }
    }
}