using Rogue.Types;
using Rogue.View.Interfaces;

namespace Rogue.Map.Interaction
{
    public class DamageInteraction
    {
        public long Damage { get; set; }

        public bool Critical { get; set; }

        public Point Location { get; set; }

        public IDrawText Custom { get; set; }
    }
}