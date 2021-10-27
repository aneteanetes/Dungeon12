using Dungeon;
using Dungeon.Localization;
using Dungeon12.Localization;

namespace Dungeon12
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
    }
}
