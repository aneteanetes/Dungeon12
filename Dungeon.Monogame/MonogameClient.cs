using Dungeon.Global;
using System;

namespace Dungeon.Monogame
{
    public class MonogameClient : IDrawFrontend
    {
        MonogameClientSettings _settings;

        public MonogameClient(MonogameClientSettings settings)
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

        public void Start()
        {
            using (var game = new XNADrawClient(_settings))
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
