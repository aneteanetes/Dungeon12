namespace Dungeon12.Abilities.Enums
{
    public enum AbilityActionType
    {
        /// <summary>
        /// Ability deal damage enemy
        /// </summary>
        Damage = 0,

        /// <summary>
        /// Ability heal character
        /// </summary>
        Heal = 1,

        /// <summary>
        /// Ability create item
        /// </summary>
        Craft = 3,

        /// <summary>
        /// Ability do something special at map
        /// </summary>
        Neutral = 4,

        /// <summary>
        /// Ability buff character
        /// </summary>
        Improve = 5,

        /// <summary>
        /// Ability summon creature
        /// </summary>
        Summon = 6,

        /// <summary>
        /// Special action of ability for blood mage
        /// </summary>
        Debuff = 7,

        /// <summary>
        /// Ability destroy somebody or something
        /// </summary>
        Destruction = 8
    }
}