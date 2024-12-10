using Dungeon;
using Dungeon.Drawing;
using Dungeon.Localization;
using Nabunassar.Entities.Enums;
using Nabunassar.Locale;
using Nabunassar.SceneObjects;
using System.Reflection;

namespace Nabunassar
{
    internal class Global : DungeonGlobal
    {
        public Global()
        {
            DefaultFontName = "Montserrat";
            BuildLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            try
            {
                ProjectPath = Directory.GetParent(BuildLocation).Parent.Parent.ToString();
            }
            catch
            {
                System.Console.WriteLine("ProjectPath variable is not detected Parent.Parent, but its ok for dev build, TODO: dev build");
            }
        }

        public static GameStrings Strings { get; set; } = new GameStrings();

        public static Game Game { get; set; }

        public override LocalizationStringDictionary GetStringsClass() => Strings;

        public override void LoadStrings(object localizationStringDictionary) { }

        public static HelpingSceneObject Helps;

        public static DrawColor CommonColor { get; } = new DrawColor(139, 107, 86);
        public static DrawColor CommonColorLight { get; } = new DrawColor(234, 186, 155);
        public static DrawColor DarkColor { get; } = new DrawColor(19, 11, 6);

        public static DrawColor FractionColorCult { get; } = new DrawColor(8, 73, 14);
        public static DrawColor FractionColorMages { get; } = new DrawColor(124, 13, 123);
        public static DrawColor FractionColorExarch { get; } = new DrawColor(239, 255, 0);
        public static DrawColor FractionColorRogues { get; } = CommonColor;
        public static DrawColor FractionColorVanguard { get; } = new DrawColor(10, 7, 105);

        public static GlobalWindows Windows { get; set; } = new GlobalWindows();
    }
}