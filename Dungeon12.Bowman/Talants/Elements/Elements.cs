using Dungeon12.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Bowman.Talants
{
    public class Elements : Talant<Bowman>
    {
        public override string Group => ArrowMakingTalants.ArrowMakingGroup;

        public override bool Activatable => true;

        public Elements(int order) : base(order)
        {
        }

        public override string[] DependsOn => new string[]
        {
            nameof(Flame)
        };
        public override int Tier => 1;

        public override string Name => "Элементальные стрелы";

        public override string Description => $"Стрелы которые выпускает лучник добавляют{Environment.NewLine} урон от элемента на котором он стоит.";

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