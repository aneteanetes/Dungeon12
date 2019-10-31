namespace Dungeon.Items.Enums
{
    using System;

    [Flags]
    public enum Stats
    {
        MainStats = 0x1,
        Defence = 0x01,
        Attack = 0x001,
        Damage = 0x0001,
        None = 0x00001
    }
}