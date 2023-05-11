namespace Dungeon.Localization
{
    public static class LocalizationStringExtensions
    {
        public static string Localized(this string value) => DungeonGlobal.GetBindedGlobal().GetStringsClass()[value];
        public static string Localized(this object value) => DungeonGlobal.GetBindedGlobal().GetStringsClass()[value];
    }
}
