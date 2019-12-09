using Dungeon12.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Bowman.Talants
{
    public class Poisoned : Talant<Bowman>
    {
        public override string Group => ArrowMakingTalants.ArrowMakingGroup;

        public override bool Activatable => true;

        public Poisoned(int order) : base(order)
        {
        }

        public override string[] DependsOn => new string[]
        {
            nameof(Elements)
        };
        public override int Tier => 2;

        public override string Name => "Отравленные стрелы";

        public override string Description => $"Все стрелы приобретают эфект яда{Environment.NewLine} постепенно наносящий урон и увечья.";

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