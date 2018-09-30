namespace Rogue.Perks
{
    using Rogue.Transactions;
    using Rogue.View.Interfaces;

    public abstract class Perk : Applicable, IDrawable
    {
        public abstract string Icon { get; }

        public abstract string Name { get; }

        public abstract IDrawColor BackgroundColor { get; set; }

        public abstract IDrawColor ForegroundColor { get; set; }

        public abstract string Description { get; }
    }
}