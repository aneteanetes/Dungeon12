using Dungeon.Monogame.Settings;
using System;

namespace Dungeon.Monogame.Runner
{
    public class MonogameRunner : IGameRunner
    {
        private readonly MonogameSettings _settings;

        public MonogameRunner(MonogameSettings settings)
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
                DungeonGlobal.OnExit += () =>
                {
                    game.Exit();
                    game.DrawClient.Dispose();
                    game.Dispose();
                };

                DungeonGlobal.OnRun?.Invoke();

                game.Run();
            }
        }
    }
}
