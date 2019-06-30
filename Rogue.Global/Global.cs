namespace Rogue
{
    using Rogue.View.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    public static class Global
    {
        public static IDrawClient DrawClient;

        public static object FreezeWorld = null;

        public static bool BlockSceneControls { get; set; }

        public static GlobalTime Time { get; } = new GlobalTime();


        public static object TransportVariable { get; set; }
    }
}