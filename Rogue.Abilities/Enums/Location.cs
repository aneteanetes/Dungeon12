namespace Rogue.Abilities.Enums
{
    public enum Location
    {
        /// <summary>
        /// Ability can be used only at world map
        /// </summary>
        WorldMap = 0,

        /// <summary>
        /// Ability can be used only in combat
        /// </summary>
        Combat = 1,

        /// <summary>
        /// Ability can be used whenever
        /// </summary>
        Alltime = 2
    }
}