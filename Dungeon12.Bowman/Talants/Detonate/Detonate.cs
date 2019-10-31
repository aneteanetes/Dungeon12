using Dungeon.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Classes.Bowman.Talants
{
    public class Detonate : Talant<Bowman>
    {
        public Detonate(int order) : base(order)
        {
        }

        public override string[] DependsOn => new string[]
        {
            nameof(Sharp),
            nameof(Incendiary)
        };
        public override int Tier => 2;

        public override string Name => "Детонация стрел";

        public override string Description => $"Некоторые стрелы застрявшие во врагах взрываются";

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
