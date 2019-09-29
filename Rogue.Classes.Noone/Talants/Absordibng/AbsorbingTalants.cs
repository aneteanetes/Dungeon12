using Rogue.Abilities.Talants;
using Rogue.Classes.Noone.Talants.Absordibng;
using System.ComponentModel.DataAnnotations;

namespace Rogue.Classes.Noone.Talants
{
    public class AbsorbingTalants : TalantTree<Noone>
    {
        public override string Name => "Поглощение";

        public override string Tileset => "";

        public AbsorbedPoison Poison { get; set; } = new AbsorbedPoison() { Level = 1 };
    }
}
