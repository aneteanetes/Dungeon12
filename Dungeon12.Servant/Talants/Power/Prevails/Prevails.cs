using Dungeon12.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Servant.Talants.Power
{
    public class Prevails : Talant<Servant>
    {
        public Prevails(int order) : base(order)
        {
        }

        public override string Name => "Преобладание";//"Молебен";

        public override int MaxLevel => 2;

        public override string Description => $"Увеличивает ресурсы получаемые от ударов";
        
        public override string[] DependsOn => new string[]
        {
            nameof(Dogma)
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
