using Rogue.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Servant.Talants.Faith
{
    public class Psalm : Talant<Servant>
    {
        public Psalm(int order) : base(order)
        {
        }

        public override string Name => "Псалм";

        public override int MaxLevel => 3;

        public override string Description => $"Атака ближайших союзников увеличивается";


        public override string[] DependsOn => new string[]
        {
            nameof(Mass)
        };

        public override int Tier => 2;

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
