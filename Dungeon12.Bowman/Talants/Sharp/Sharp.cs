using Rogue.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Classes.Bowman.Talants
{
    public class Sharp : Talant<Bowman>
    {
        public Sharp(int order) : base(order)
        {
        }

        public override string[] DependsOn => new string[]
        {
            nameof(Lightweight)
        };
        public override int Tier => 1;

        public override string Name => "Заострённые стрелы";

        public override string Description => $"Все стрелы получают шанс застрять в целях.";

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
