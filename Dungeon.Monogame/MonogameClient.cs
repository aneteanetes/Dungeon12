using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Monogame
{
    public class MonogameClient
    {
        MonogameClientSettings _settings;

        public MonogameClient(MonogameClientSettings settings) => _settings = settings;

        public void Run(bool FATAL = false)
        {
            using (var game = new XNADrawClient(_settings))
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
