using Rogue.Transactions;

namespace Rogue.Classes.Paladin.Perks
{
    public class PaladinPerk : Applicable
    {
        public void Apply(Paladin paladin)
        {
            paladin.AbilityPower += 1;
        }

        public void Discard(Paladin paladin)
        {
            paladin.AbilityPower -= 1;
        }

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
