namespace Rogue.Entites.Items
{
    using Rogue.Entites.Alive;
    using Rogue.Items;
    using Rogue.Transactions;

    public class Equipment : Applicable
    {
        public Item Item { get; set; }

        public Equipment(Item item)
        {
            this.Item = item;
        }

        public void Apply(Character @char)
        {
            @char.MaxHitPoints += 1000;
        }

        public void Discard(Character @char)
        {
            @char.MaxHitPoints -= 1000;
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