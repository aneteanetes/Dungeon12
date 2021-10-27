namespace Dungeon12.Abilities.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum Location
    {
        /// <summary>
        /// Ability can be used only at world map
        /// </summary>
        [Display(Name = "В городе")]
        WorldMap = 0,

        /// <summary>
        /// Ability can be used only in combat
        /// </summary>
        [Display(Name = "За городом")]
        Combat = 1,

        /// <summary>
        /// Ability can be used whenever
        /// </summary>
        [Display(Name = "Везде")]
        Alltime = 2,

        /// <summary>
        /// Ability can be used whenever
        /// </summary>
        [Display(Name = "В бою")]
        OnlyCombat = 3
    }
}