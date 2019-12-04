namespace Dungeon
{
    using Dungeon.Audio;
    using Dungeon.Control;
    using Dungeon.Data;
    using Dungeon.Events;
    using Dungeon.Game;
    using Dungeon.Logging;
    using Dungeon.SceneObjects;
    using Dungeon.Scenes.Manager;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public static class Global
    {
        static Global()
        {
            TimeTrigger.GlobalTimeSource = () => Time;
            Logger.InitGlobalHandling();
        }

        public static string Version { get; set; } = "0.0.2";

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

        public static SceneManager SceneManager { get; set; }

        public static GameState GameState { get; set; } = new GameState();

        public static void Exception(Exception ex, Action ok=default)
        {
            Logger.Log(ex.ToString());
            try
            {
                Save(0, "Автосохранение");
                MessageBox.Show($"Ошибка!{Environment.NewLine}Игра сохранена, выйдите и загрузите игру снова!", ok);
            }
            catch (Exception ex1)
            {
                Logger.Log(ex1.ToString());
                MessageBox.Show($"Ошибка!{Environment.NewLine} Игра НЕ СОХРАНЕНА, выйдите и загрузите игру снова!", ok);
            }
        }

        public static string Save(int id=0, string name="") => Database.Save(id,name);

        /// <summary>
        /// Сохраняет текущий регион в память
        /// </summary>
        public static void SaveInMemmory() => Database.Save(0, "@!#$memory$#!@", true, true);

        /// <summary>
        /// Удаляет текущую карту из памяти
        /// </summary>
        public static void RemoveSaveInMemmory()
        {
            var temp = Load("@!#$memory$#!@");
            if (temp != default)
                Database.RemoveSavedGame(Load("@!#$memory$#!@").Id);
        }

        public static SaveModel Load(string id) => Database.Load(id);

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