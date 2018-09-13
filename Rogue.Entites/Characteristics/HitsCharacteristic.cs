using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Entites.Structural;

namespace Rogue.Entites.Characteristics
{
    public class HitsCharacteristic : Characteristic<long>
    {
        public HitsCharacteristic() : base("HP") { }

        protected override void CallApply(dynamic obj)
        {
            this.Apply(obj);
        }

        protected override void CallDiscard(dynamic obj)
        {
            this.Discard(obj);
        }
    }
}
