using Dungeon.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Servant.Talants.Power
{
    public class Litany : Talant<Servant>
    {
        public Litany(int order) : base(order)
        {
        }

        public override string Name => "Литания";//"Молебен";

        public override string Description => $"Наносит удар в ближнем бою";

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
