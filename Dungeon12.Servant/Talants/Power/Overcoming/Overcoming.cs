using Dungeon.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Classes.Servant.Talants.Power
{
    public class Overcoming : Talant<Servant>
    {
        public override bool Activatable => true;

        public override string Group => PowerTalants.Mage;

        public Overcoming(int order) : base(order)
        {
        }

        public override int MaxLevel => 3;

        public override string Name => "Преодоление";//"Молебен";

        public override string Description => $"Сила веры рождается от полученного урона";
        
        public override string[] DependsOn => new string[]
        {
            nameof(Litany)
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
