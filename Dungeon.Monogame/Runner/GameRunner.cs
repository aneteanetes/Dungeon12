using Dungeon.Global;
using Dungeon.Monogame.Settings;
using System;

namespace Dungeon.Monogame.Runner
{
    public class GameRunner : IGameRunner
    {
        private readonly GameSettings _settings;

        public GameRunner(GameSettings settings)
        {
            _settings = settings;

            DungeonGlobal.Sizes.Width = settings.WidthPixel;
            DungeonGlobal.Sizes.Height = settings.HeightPixel;

            DungeonGlobal.Resolution = new View.PossibleResolution()
            {
                Width = settings.WidthPixel,
                Height = settings.HeightPixel
            };
        }

        public void Run()
        {
            using (var game = new GameClient(_settings))
            {
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
