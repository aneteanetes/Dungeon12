namespace Rogue.Items.Types
{
    using System;
    using Rogue.Items.Enums;

    public class Elixir : Item
    {
        public DateTime Duration;

        public override Stats AvailableStats => Stats.Attack & Stats.Defence;
    }
}