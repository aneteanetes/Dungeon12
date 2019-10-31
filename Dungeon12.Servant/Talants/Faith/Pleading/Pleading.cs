using Rogue.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Classes.Servant.Talants.Faith
{
    public class Pleading : Talant<Servant>
    {
        public Pleading(int order) : base(order)
        {
        }

        public override string Name => "Мольба";


        public override int MaxLevel => 4;

        public override string Description => $"Уменьшает период получения веры во время молитвы";

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
