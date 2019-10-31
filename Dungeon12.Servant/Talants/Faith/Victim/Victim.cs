using Dungeon.Abilities.Talants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Classes.Servant.Talants.Faith
{
    public class Victim : Talant<Servant>
    {
        public Victim(int order) : base(order)
        {
        }

        public override string Name => "Жертва";

        public override int MaxLevel => 1;

        public override string Description => $"Исцеление эффективнее в 4 раза, {Environment.NewLine} но половина исцеленного урона наносится служителю.";


        public override string[] DependsOn => new string[]
        {
            nameof(Divinity)
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
