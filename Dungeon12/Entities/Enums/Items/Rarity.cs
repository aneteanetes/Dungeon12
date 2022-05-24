using Dungeon;

namespace Dungeon12.Entities.Enums
{
    internal enum Rarity
    {
        [DrawColour(157,157,157)]
        Poor,
        [DrawColour(255, 255, 255)]
        Common,
        [DrawColour(30, 255, 0)]
        Uncommon,
        [DrawColour(0, 112, 221)]
        Rare,
        [DrawColour(163, 53, 238)]
        Epic,
        [DrawColour(255, 128, 0)]
        Legendary
    }
}