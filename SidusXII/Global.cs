using Dungeon;
using Dungeon.Localization;
using SidusXII.Localization;
using SidusXII.Settings;

namespace SidusXII
{
    public class Global : DungeonGlobal
    {
        public Global()
        {
            DefaultFontName = "Montserrat";
            BuildLocation = @"C:\Users\anete\source\repos\Dungeon12\SidusXII\bin\Debug\netcoreapp3.1";
        }

        public static GameStrings Strings { get; set; } = new GameStrings();

        public override LocalizationStringDictionary GetStringsClass() => Strings;

        public override void LoadStrings(object localizationStringDictionary) { }

        public static Game Game { get; set; }

        public static RootSettings Settings = new RootSettings();
    }
}
