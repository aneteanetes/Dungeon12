using Dungeon.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Servant.Talants.Faith
{
    public class Light : Talant<Servant>
    {
        public override bool Activatable => true;

        public Light(int order) : base(order)
        {
        }

        public override string Name => "Свет";

        public override int MaxLevel => 1;

        public override string[] DependsOn => new string[]
        {
            nameof(Mass)
        };

        public override int Tier => 2;

        public override string Description => $"Освещение стоит 1 силу веры,{Environment.NewLine} даёт свет, но эффективность {Environment.NewLine}падает в три раза.";

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
