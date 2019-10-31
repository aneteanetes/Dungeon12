using Rogue.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Classes.Bowman.Talants
{
    public class Piercing : Talant<Bowman>
    {
        public override string Group => ArrowMakingTalants.ArrowMakingGroup;

        public override bool Activatable => true;

        public Piercing(int order) : base(order)
        {
        }

        public override int Tier => 3;

        public override string Name => "Пронзающие стрелы";

        public override string Description => $"Все стрелы получают шанс бить на вылет.";

        protected override void CallApply(dynamic obj)
        {
            return;
        }

        protected override bool CallCanUse(dynamic obj)
        {
            return true;
        }

        protected override void CallDiscard(dynamic obj)
        {
            return;
        }

        protected override TalantInfo CallTalantInfo(dynamic obj)
        {
            return new TalantInfo();
        }
    }
}
