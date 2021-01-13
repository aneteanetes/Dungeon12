namespace Dungeon
{
    using Dungeon.Audio;
    using Dungeon.Control;
    using Dungeon.Drawing;
    using Dungeon.Events;
    using Dungeon.Global;
    using Dungeon.Logging;
    using Dungeon.Resources;
    using Dungeon.Scenes.Manager;
    using Dungeon.Settings;
    using Dungeon.View.Interfaces;
    using MoreLinq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class DungeonGlobal
    {
        public DungeonGlobal()
        {
            TimeTrigger.GlobalTimeSource = () => Time;
        }

        private static DungeonGlobal global;

        public static void BindGlobal<T>() where T: DungeonGlobal
        {
            ResourceLoader.LoadAllAssembliesInFolder();
            GlobalExceptionHandling();
            global = typeof(T).NewAs<T>();
        }

        /// <summary>
        /// Если установлено true тогда <see cref="ISceneObject.Update(GameTimeLoop)"/> будет работать только на компонентах у которых включён <see cref="ISceneObject.Updatable"/> и в зависимости от дерева композиции
        /// </summary>
        public static bool ComponentUpdateCompatibility { get; set; }

        public static string Platform { get; set; } = "win";

        public static string Version { get; set; } = "0.0.7";

        public static Logger Logger { get; set; } = new Logger();

        public static ICamera Camera { get; set; }

        public static IDrawClient DrawClient { get; set; }

        public static Freezer Freezer { get; set; } = new Freezer(SceneManager);

        public static AudioOptions AudioOptions { get; set; } = new AudioOptions();

        public static IAudioPlayer AudioPlayer { get; set; }

        public static bool BlockSceneControls { get; set; }

        public static GameTime Time { get; } = new GameTime();

        public static EventBus Events { get; } = new EventBus();

        public static object TransportVariable { get; set; }

        public static PointerArgs PointerLocation { get; set; }

        public static double FPS { get; set; }

        public static bool Interacting { get; set; }

        public static DungeonAssemblyContext DungeonAssemblyContext { get; set; }

        internal static List<Assembly> StaticAssemblies = new List<Assembly>();

        public static IEnumerable<Assembly> Assemblies => StaticAssemblies.Concat(DungeonAssemblyContext?.Assemblies ?? new Assembly[0]);

        public static string GameAssemblyName { get; set; }

        public static Assembly GameAssembly { get; set; }

        private static string _gameTitle;
        public static string GameTitle
        {
            get
            {
                if(_gameTitle==default)
                {
                    return GameAssemblyName;
                }

                return _gameTitle;
            }
            set => _gameTitle = value;
        }

        public static string DefaultFontName { get; set; } = "FledglingSb";

        public static int DefaultFontSize { get; set; } = 14;

        public static SceneManager SceneManager { get; set; }

        /// <summary>
        /// Вместо обработки ошибок - падать
        /// </summary>
        public static bool ExceptionRethrow { get; set; } = false;
        
        protected virtual void OnException(Exception ex, Action ok=default) { }

        public static void Exception(Exception ex, Action ok=default)
        {
            global.OnException(ex, ok);

            if (ExceptionRethrow)
                throw ex;

            Logger.Log(ex.ToString());
        }

        public static void GlobalExceptionHandling()
        {                
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalHandler);
            void GlobalHandler(object sender, UnhandledExceptionEventArgs args)
            {
                Exception e = (Exception)args.ExceptionObject;
                Logger.Log(e.ToString());
                if (!Directory.Exists("Crashes"))
                {
                    Directory.CreateDirectory("Crashes");
                }
                Logger.Save($"Crashes\\{DateTime.Now.ToString("dd-MM-yyyy HH_mm")}.txt");
            }
        }
        
        public static Action Exit { get; set; } = () =>
        {
            if(!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
            Logger.SaveIsNeeded($"Logs\\{DateTime.Now.ToString("dd=MM HH_mm_ss")}");
        };

        public static DrawClientRunDelegate ClientRun;

        public static DrawingSize Sizes { get; set; } = new DrawingSize();

        public static void Run(bool FATAL = false)
        {
            try
            {
                ClientRun(FATAL);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                throw;
                Run(true);
            }
        }
    }
}