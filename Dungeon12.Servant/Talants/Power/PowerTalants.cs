using Dungeon12.Abilities.Talants;
using Dungeon12.Servant.Talants.Power;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Servant.Talants
{
    public class PowerTalants : TalantTree<Servant>
    {
        public const string Warrior = "Служба";
        public const string Mage = "Молебен";

        public override string Name => "Сила";

        public override string Tileset => "";

        public Litany Litany { get; set; } = new Litany(0);

        public Dogma Dogma { get; set; } = new Dogma(1);

        public Overcoming Overcoming { get; set; } = new Overcoming(0);

        public Prevails Prevails { get; set; } = new Prevails(1);
        
        public Worship Worship { get; set; } = new Worship(0);

        public Smite Smite { get; set; } = new Smite(1);
    }
}
