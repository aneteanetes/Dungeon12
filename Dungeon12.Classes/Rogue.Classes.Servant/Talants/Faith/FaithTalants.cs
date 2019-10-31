using Rogue.Abilities.Talants;
using Rogue.Classes.Servant.Talants.Faith;
using Rogue.Classes.Servant.Talants.Power;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Classes.Servant.Talants
{
    public class FaithTalants : TalantTree<Servant>
    {
        public override string Name => "Вера";

        public override string Tileset => "";

        public Holy Holy { get; set; } = new Holy(0);

        public Healing Healing { get; set; } = new Healing(1);

        public Pleading Pleading { get; set; } = new Pleading(2);

        public Divinity Divinity { get; set; } = new Divinity(0);

        public Mass Mass { get; set; } = new Mass(1);

        public Victim Victim { get; set; } = new Victim(0);

        public Light Light { get; set; } = new Light(1);

        public Psalm Psalm { get; set; } = new Psalm(2);
    }
}
