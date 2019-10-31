using Rogue.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Classes.Servant.Talants.Power
{
    public class Dogma : Talant<Servant>
    {
        public Dogma(int order) : base(order)
        {
        }

        public override string Name => "Догмат";

        public override string Description => $"Наносит урон в дальнем бою";

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
