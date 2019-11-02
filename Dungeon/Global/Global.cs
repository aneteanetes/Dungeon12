namespace Dungeon
{
    using Dungeon.Audio;
    using Dungeon.Control.Pointer;
    using Dungeon.Events;
    using Dungeon.View.Interfaces;
    using System.Collections.Generic;
    using System.Reflection;

    public static class Global
    {
        static Global()
        {
            TimeTrigger.GlobalTimeSource = () => Time;
        }

        public static IDrawClient DrawClient;

        private static object freezeWorldObject;
        public static object FreezeWorld
        {
            get => freezeWorldObject;
            set
            {
                if (value == null)
                {
                    Time.Resume();
                }
                else
                {
                    Time.Pause();
                }

                freezeWorldObject = value;
            }
        }

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
    }
}