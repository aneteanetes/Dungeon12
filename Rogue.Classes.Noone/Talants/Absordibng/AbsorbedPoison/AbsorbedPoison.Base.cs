using Rogue.Abilities;
using Rogue.Abilities.Talants;
using Rogue.Classes.Noone.Abilities;
using Rogue.Drawing.GUI;
using Rogue.Entites.Enemy;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.Transactions;
using Rogue.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Noone.Talants.Absordibng
{
    public partial class AbsorbedPoison : Talant<Noone>
    {
        public AbsorbedPoison(int order):base(order)
        {

        }

        public override string Group => nameof(Absorbing);
        public override bool Activatable => true;

        public override string Name => "Поглощение яда";

        public override string Description => "Открывает возможность использовать поглощённые свойства яда";

        public override int Tier => 1;

        protected override void CallApply(dynamic obj)
        {
            this.Apply(obj);
        }

        protected override bool CallCanUse(dynamic obj)
        {
            return this.CanUse(obj);
        }

        protected override void CallDiscard(dynamic obj)
        {
            this.Discard(obj);
        }

        protected override TalantInfo CallTalantInfo(dynamic obj)
        {
            return this.TalantInfo(obj);
        }

        public override string[] DependsOn => new string[]
        {
            nameof(Absorbing)
        };
    }
}
