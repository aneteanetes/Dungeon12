﻿using Dungeon.Audio;
using Dungeon.Configuration;
using Dungeon.Control;
using Dungeon.Events;
using Dungeon.Global;
using Dungeon.Localization;
using Dungeon.Logging;
using Dungeon.Resources;
using Dungeon.Scenes.Manager;
using Dungeon.Settings;
using Dungeon.View;
using Dungeon.View.Interfaces;
using Microsoft.Extensions.Configuration;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;

namespace Dungeon
{
    public abstract class DungeonGlobal
    {
        public DungeonGlobal()
        {
            TimeTrigger.GlobalTimeSource = () => Time;
        }

        private static DungeonGlobal global;

        public static DungeonGlobal GetBindedGlobal() => global;

        public static bool IsDevelopment { get; private set; }

        private static bool isInited = false;

        public static DungeonConfiguration Init<T>(bool isDevelop, bool compileData=false) where T : DungeonGlobal
        {
            ResourceLoader.ResourceResolvers.Add(new EmbeddedResourceResolver(Assembly.GetEntryAssembly()));
            Console.OutputEncoding = Encoding.UTF8;
            IsDevelopment=isDevelop;

            DungeonGlobal.GameAssembly = typeof(T).Assembly;
            DungeonGlobal.GameAssemblyName = DungeonGlobal.GameAssembly.GetName().Name;

            ResourceLoader.LoadAllAssembliesInFolder();
            GlobalExceptionHandling();
            global = typeof(T).NewAs<T>();

            if (compileData)
            {
                var resCompiler = new ResourceCompiler(logOnlyNewUpdate:true);
                resCompiler.Compile();
            }

            var strings = global.GetStringsClass();
            if (strings != default)
            {
                var loaded = strings.___AutoLoad(strings.___DefaultLanguageCode, strings);
                if (loaded != default)
                {
                    global.LoadStrings(loaded);
                }
            }

            var config = new ConfigurationBuilder()
                .AddJsonFile($"{GameAssemblyName.ToLowerInvariant()}.cfg", true)
                .AddJsonFile($"{GameAssemblyName.ToLowerInvariant()}.local.cfg", true)
                .Build();

            Configuration = config.Get<DungeonConfiguration>();
            Configuration.ConfigurationRoot = config;
            ResourceLoader.Settings = Configuration.ResourceLoader;

            isInited = true;

            return Configuration;
        }

        public static DungeonConfiguration Configuration { get; private set; }

        /// <summary>
        /// Если установлено true тогда <see cref="ISceneObject.ComponentUpdateChainCall(GameTimeLoop)"/> будет работать только на компонентах у которых включён <see cref="ISceneObject.Updatable"/> и в зависимости от дерева композиции
        /// </summary>
        public static bool ComponentUpdateCompatibility { get; set; }

        public static string Platform { get; set; } = "win";

        public static Version Version { get; set; } = new Version();

        public static Logger Logger { get; set; } = new Logger();

        public static ICamera Camera { get; set; }

        public static IGameClient GameClient { get; set; }

        private static Freezer freezer;
        public static Freezer Freezer
        {
            get
            {
                if (freezer == null)
                {
                    freezer = new Freezer(SceneManager);
                }

                return freezer;
            }
        }

        public static AudioOptions AudioOptions { get; set; } = new AudioOptions();

        public static IAudioPlayer AudioPlayer { get; set; }

        public static bool BlockSceneControls { get; set; }

        public static InGameTime Time { get; } = new InGameTime();

        public static EventBus Events { get; } = new EventBus();

        public static object TransportVariable { get; set; }

        public static PossibleResolution Resolution { get; set; }

        public static Matrix4x4 ResolutionScaleMatrix { get; set; }

        public static Action<PossibleResolution> ChangeResolution { get; set; }

        public static PointerArgs PointerLocation { get; set; }

        public static double FPS { get; set; }

        public static bool Interacting { get; set; }

        public static DungeonAssemblyContext DungeonAssemblyContext { get; set; }

        internal static List<Assembly> StaticAssemblies = new List<Assembly>();

        public static IEnumerable<Assembly> Assemblies => StaticAssemblies.Concat(DungeonAssemblyContext?.Assemblies ?? new Assembly[0]);

        public static string GameAssemblyName { get; set; }

        public static Assembly GameAssembly { get; set; }

        public static string BuildLocation { get; set; }
        
        public static string ProjectPath { get; set; }

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
                ExceptionDispatchInfo.Capture(ex).Throw();

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

        public static Action OnRun { get; set; }

        public static bool GamePadConnected { get; set; } = false;

        public static DrawingSize Sizes { get; set; } = new DrawingSize();

        public static CultureInfo CultureInfo { get; set; }

        public static void SetCulture(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
            var global = GetBindedGlobal();
            var strings = global.GetStringsClass();
            global.LoadStrings(strings.___AutoLoad(cultureInfo.TwoLetterISOLanguageName));
        }

        public abstract LocalizationStringDictionary GetStringsClass();

        public abstract void LoadStrings(object localizationStringDictionary);

        public string this[string index] => GetStringsClass()[index];

        public static void Run(IGameRunner drawFrontend)
        {
            if (!isInited)
                throw new Exception("Dungeon.Init must call before run!");
            drawFrontend.Run();
        }
    }
}