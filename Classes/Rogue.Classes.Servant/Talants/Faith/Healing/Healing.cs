using Rogue.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Servant.Talants.Faith
{
    public class Healing : Talant<Servant>
    {
        public Healing(int order) : base(order)
        {
        }

        public override string Name => "Лечение";

        public override string Description => $"Увеличивает эффективность исцеления в два раза";


        public override int MaxLevel => 1;

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
