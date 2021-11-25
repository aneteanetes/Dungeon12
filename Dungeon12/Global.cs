using System.Reflection;
using System.IO;
using Dungeon;
using Dungeon.Localization;
using Dungeon12.Localization;
using System.Collections.Generic;
using Dungeon12.Functions;
using Dungeon.Scenes;
using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.Map;

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

        public static Game Game { get; set; }

        public override LocalizationStringDictionary GetStringsClass() => Strings;

        public override void LoadStrings(object localizationStringDictionary) { }

        public static Dictionary<string, IFunction> Functions = new Dictionary<string, IFunction>();

        public static HintScenarioSceneObject Hints;

        public static bool RegisterFunction<TFuncClass>() where TFuncClass : IFunction
        {
            var func = typeof(TFuncClass).NewAs<TFuncClass>();
            return Functions.TryAdd(func.Name, func);
        }

        public static bool ExecuteFunction(ISceneLayer sceneLayer, string name)
        {
            if (Functions.TryGetValue(name, out var func))
            {
                return func.Call(sceneLayer);
            }

            return false;
        }
    }
}
