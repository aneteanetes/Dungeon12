namespace Dungeon12.Entities.Enums
{
    public enum AbilRange
    {
        Close,
        Far,
        Any,
        Friendly,
        Summon,
        Weapon
    }

    public static class RangeExtensions
    {
        public static string Display(this AbilRange range)
        {
            switch (range)
            {
                case AbilRange.Close: return Global.Strings["RangeClose"];
                case AbilRange.Far: return Global.Strings["RangeFar"];
                case AbilRange.Any: return Global.Strings["RangeAny"];
                case AbilRange.Friendly: return Global.Strings["RangeFriendly"];
                case AbilRange.Summon: return Global.Strings["Summon"];
                case AbilRange.Weapon: return Global.Strings["RangeWeaponDepends"];
                default: return "";
            }
        }
    }
}