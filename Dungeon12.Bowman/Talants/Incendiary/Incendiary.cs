using Dungeon.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Bowman.Talants
{
    public class Incendiary : Talant<Bowman>
    {
        public override string Group => ArrowMakingTalants.ArrowMakingGroup;

        public override bool Activatable => true;

        public Incendiary(int order) : base(order)
        {
        }

        public override string[] DependsOn => new string[]
        {
            nameof(Flame)
        };
        public override int Tier => 1;

        public override string Name => "Поджигающие стрелы";

        public override string Description => $"Такие стрелы поджигают противника и наносят урон{Environment.NewLine} однако если враг уже горит, наносят двойной урон.";

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