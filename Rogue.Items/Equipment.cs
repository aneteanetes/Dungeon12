namespace Rogue.Items
{
    using Rogue.Transactions;
    using Rogue.View.Interfaces;

    public abstract class Equipment : Applicable
    {
        public string Title { get; set; }

        public string Value { get; set; }

        public IDrawColor Color { get; set; }
    }
}