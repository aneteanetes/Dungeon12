namespace Dungeon
{
    using Dungeon.Audio;
    using Dungeon.Control;
    using Dungeon.Events;
    using Dungeon.Logging;
    using Dungeon.Scenes.Manager;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
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
            GlobalExceptionHandling();
            global = typeof(T).NewAs<T>();
        }

        public static string Platform { get; set; } = "win";

        public static string Version { get; set; } = "0.0.3";

        public static Logger Logger { get; set; } = new Logger();

        public static ICamera Camera { get; set; }

        public static IDrawClient DrawClient { get; set; }

        public static Freezer Freezer { get; set; } = new Freezer();

        public static IAudioPlayer AudioPlayer { get; set; }

        public static bool BlockSceneControls { get; set; }

        public static GameTime Time { get; } = new GameTime();

        public static EventBus Events { get; } = new EventBus();

        public static object TransportVariable { get; set; }

        public static PointerArgs PointerLocation { get; set; }

        public static double FPS { get; set; }

        public static bool Interacting { get; set; }

        public static IEnumerable<Assembly> Assemblies { get; set; }

        public static string GameAssemblyName { get; set; }

        public static Assembly GameAssembly { get; set; }

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
    }
}