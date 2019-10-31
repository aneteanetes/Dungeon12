using Rogue.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Classes.Bowman.Talants
{
    public class ArrowMakingTalants : TalantTree<Bowman>
    {
        public const string ArrowMakingGroup = "Активные стрелы";

        public override string Name => "Изготовление стрел";

        public override string Tileset => "";

        public Flame Flame { get; set; } = new Flame(0);

        public Lightweight Lightweight { get; set; } = new Lightweight(1);

        public Piercing Piercing { get; set; } = new Piercing(2);

        public Sharp Sharp { get; set; } = new Sharp(3);

        public Incendiary Incendiary { get; set; } = new Incendiary(1);

        public Elements Elements { get; set; } = new Elements(0);

        public Poisoned Poisoned { get; set; } = new Poisoned(0);

        public Detonate Detonate { get; set; } = new Detonate(1);
    }
}
