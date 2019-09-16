namespace Rogue.Classes.Noone
{
    using Rogue.Items;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ParryEquip : Equipment
    {
        public override string Title => $"+12 Паррирование";

        public void Apply(Character character)
        {

        }

        public void Discard(Character character)
        {

        }

        protected override void CallApply(dynamic obj) => this.Apply(obj);
        protected override void CallDiscard(dynamic obj) => this.Discard(obj);
    }
}