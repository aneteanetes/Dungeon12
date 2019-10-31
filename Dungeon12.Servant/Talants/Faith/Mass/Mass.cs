using Dungeon.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Servant.Talants.Faith
{
    public class Mass : Talant<Servant>
    {
        public Mass(int order) : base(order)
        {
        }

        public override string Name => "Месса";


        public override int MaxLevel => 3;

        public override string Description => $"Во время молитвы защита увеличивается";
        
        public override string[] DependsOn => new string[]
        {
            nameof(Pleading),
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
