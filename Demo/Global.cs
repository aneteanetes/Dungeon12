using Dungeon;
using Dungeon.Localization;

namespace Demo
{
    public class Global : DungeonGlobal
    {
        public Global()
        {
            DefaultFontName = "Montserrat";
        }

        public override LocalizationStringDictionary GetStringsClass() => default;

        public override void LoadStrings(object localizationStringDictionary) { }
    }
}
