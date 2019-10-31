using Dungeon.Abilities;
using Dungeon.Abilities.Talants;
using Dungeon.Classes.Noone.Abilities;
using Dungeon.Drawing.GUI;
using Dungeon.Entites.Enemy;
using Dungeon.Map;
using Dungeon.Map.Objects;
using Dungeon.Transactions;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Classes.Noone.Talants.Absordibng
{
    public partial class AbsorbedPoison : Talant<Noone>
    {
        public AbsorbedPoison(int order):base(order)
        {

        }

        public override string Group => Absorbing.GroupName;
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
