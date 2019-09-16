namespace Rogue
{
    using Rogue.Audio;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Global
    {
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

        public static GlobalTime Time { get; } = new GlobalTime();


        public static object TransportVariable { get; set; }
    }
}