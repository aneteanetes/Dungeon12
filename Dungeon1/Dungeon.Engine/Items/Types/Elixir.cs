namespace Dungeon.Items.Types
{
    using System;
    using Dungeon.Items.Enums;

    public class Elixir : Item
    {
        public DateTime Duration;

        public override Stats AvailableStats => Stats.Attack & Stats.Defence;
    }
}