namespace Dungeon12.Items.Types
{
    using System;
    using Dungeon12.Items.Enums;

    public class Elixir : Item
    {
        public DateTime Duration;

        public override Stats AvailableStats => Stats.Attack & Stats.Defence;
    }
}