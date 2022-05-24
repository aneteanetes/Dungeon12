using Dungeon;

namespace Dungeon12.Entities.Enums
{
    internal enum ItemMaterial
    {
        [DrawColour(184,115,51)]
        Copper, 
        [DrawColour(169, 113, 66)]
        Bronze,
        [DrawColour(123, 144, 149)]
        Steel,

        /// <summary>
        /// Хлопок
        /// </summary>
        [DrawColour(253, 243, 234)]
        Cotton,
        [DrawColour(249, 237, 228)]
        Wool,
        [DrawColour(255, 221, 202)]
        Silk,

        /// <summary>
        /// сырая кожа
        /// </summary>
        [DrawColour(186, 154, 121)]
        RawLeather,
        /// <summary>
        /// Сыромятная кожа
        /// </summary>
        [DrawColour(151, 105, 55)]
        Rawhide,
        /// <summary>
        /// Дублёная кожа
        /// </summary>
        [DrawColour(98, 74, 46)]
        TannedLeather
    }
}