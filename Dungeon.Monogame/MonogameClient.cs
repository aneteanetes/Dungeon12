using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Monogame
{
    public static class MonogameClient
    {
        public static void Run(bool FATAL = false)
        {
            using (var game = new XNADrawClient())
            {
                game.isFatal = FATAL;
                DungeonGlobal.Exit += () =>
                {
                    game.Dispose();
                    game.Exit();
                    Environment.Exit(0);
                };
                game.Run();
            }
        }
    }
}
