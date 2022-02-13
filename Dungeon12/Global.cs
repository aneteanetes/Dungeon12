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

        public static HelpingSceneObject Helps;

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