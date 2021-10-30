using System.Reflection;
using System.IO;
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
            BuildLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            ProjectPath = Directory.GetParent(BuildLocation).Parent.Parent.ToString();
        }

        public static GameStrings Strings { get; set; } = new GameStrings();

        public override LocalizationStringDictionary GetStringsClass() => Strings;

        public override void LoadStrings(object localizationStringDictionary) { }
    }
}
