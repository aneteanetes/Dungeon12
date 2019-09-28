using Rogue.Abilities.Talants;
using Rogue.Classes.Noone.Talants.Absordibng;
using System.ComponentModel.DataAnnotations;

namespace Rogue.Classes.Noone.Talants
{
    [Display(Name ="Поглощение")]
    public class AbsorbingTalants : TalantTree<Noone>
    {
        public AbsorbedPoison Poison { get; set; } = new AbsorbedPoison() { Level = 1 };
    }
}
