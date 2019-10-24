using Rogue.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Bowman.Talants
{
    public class Flame : Talant<Bowman>
    {
        public override string Group => ArrowMakingTalants.ArrowMakingGroup;

        public override bool Activatable => true;

        public Flame(int order) : base(order)
        {
        }

        public override string Name => "Горящие стрелы";

        public override string Description => $"Позволяет поджигать все стрелы";

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
