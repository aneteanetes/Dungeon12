using Rogue.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Classes.Bowman.Talants
{
    public class Lightweight : Talant<Bowman>
    {
        public override string Group => ArrowMakingTalants.ArrowMakingGroup;

        public override bool Activatable => true;

        public Lightweight(int order) : base(order)
        {
        }

        public override string Name => "Облегчённые стрелы";

        public override string Description => $"Стрелы становятся легче и летят дальше.";

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
