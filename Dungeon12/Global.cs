using System.Reflection;
using System.IO;
using Dungeon;
using Dungeon.Localization;
using Dungeon12.Localization;
using System.Collections.Generic;
using Dungeon12.Functions;
using Dungeon.Scenes;
using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects;
using System.Linq;
using Dungeon12.Entities.Enums;
using Dungeon.Drawing;

namespace Dungeon12
{
    public class Global : DungeonGlobal
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

        public static Dictionary<string, IFunction> Functions = new Dictionary<string, IFunction>();

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

        public static bool RegisterFunction<TFuncClass>() where TFuncClass : IFunction
        {
            var func = typeof(TFuncClass).NewAs<TFuncClass>();
            return Functions.TryAdd(func.Name, func);
        }

        public static bool ExecuteFunction(ISceneLayer sceneLayer, string name, string objectId)
        {
            if (Functions.TryGetValue(name, out var func))
            {
                return func.Call(sceneLayer, objectId);
            }

            return false;
        }

        public static Spec DemoSpecNPC() => Game.Party.Hero1.Spec switch
        {
            Spec.WarriorWarchief => Spec.PaladinAdept,
            Spec.MageAoe => Spec.MercenaryLeader,
            Spec.MercenaryLeader => Spec.MageAoe,
            Spec.PaladinAdept => Spec.WarriorWarchief,
            Spec.WarlockNecromancer => Spec.WarriorWarchief,
            _ => Spec.InquisitorJudge,
        };
    }
}