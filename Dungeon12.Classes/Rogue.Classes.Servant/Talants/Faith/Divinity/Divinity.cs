using Rogue.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Classes.Servant.Talants.Faith
{
    public class Divinity : Talant<Servant>
    {
        public Divinity(int order) : base(order)
        {
        }

        public override string Name => "Божественность";

        public override string Description => $"Освещение лечит союзников.";

        public override int MaxLevel => 1;


        public override string[] DependsOn => new string[]
        {
            nameof(Holy),
            nameof(Healing)
        };

        public override int Tier => 1;

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
