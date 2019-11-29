namespace Dungeon
{
    using Dungeon.Audio;
    using Dungeon.Control;
    using Dungeon.Data;
    using Dungeon.Events;
    using Dungeon.Game;
    using Dungeon.Scenes.Manager;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static class Global
    {
        static Global()
        {
            TimeTrigger.GlobalTimeSource = () => Time;
        }

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

        public static SceneManager SceneManager { get; set; }

        public static GameState GameState { get; set; } = new GameState();

        public static string Save(int id=0) => Database.Save(id);

        /// <summary>
        /// Сохраняет текущий регион в память
        /// </summary>
        public static void SaveInMemmory() => Database.Save(0, "@!#$memory$#!@", true, true);

        public static SaveModel Load(string id) => Database.Load(id);

        public static Action Exit { get; set; }
    }
}