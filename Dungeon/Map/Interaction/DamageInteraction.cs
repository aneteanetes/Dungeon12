using Dungeon.Types;
using Dungeon.View.Interfaces;

namespace Dungeon.Map.Interaction
{
    public class DamageInteraction
    {
        public long Damage { get; set; }

        public bool Critical { get; set; }

        public Point Location { get; set; }

        public IDrawText Custom { get; set; }
    }
}