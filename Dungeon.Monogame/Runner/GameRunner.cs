using Dungeon.Global;
using Dungeon.Monogame.Settings;
using System;
using System.Threading.Tasks;

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
                    game.Exit();
                    game.DrawClient.Dispose();
                    game.Dispose();
                    Environment.Exit(0);
                };

                DungeonGlobal.OnRun?.Invoke();

                game.Run();
            }
        }
    }
}
