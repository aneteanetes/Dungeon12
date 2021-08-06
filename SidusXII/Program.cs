using Dungeon;
using Dungeon.Monogame;
using Dungeon.Resources;
using System;

namespace SidusXII
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            DungeonGlobal.BindGlobal<Global>();
            DungeonGlobal.ComponentUpdateCompatibility = true;
            Console.WriteLine(DungeonGlobal.Version);
#if DEBUG
            var resCompiler = new ResourceCompiler();
            resCompiler.Compile();

            DungeonGlobal.ExceptionRethrow = true;
            DungeonGlobal.GlobalExceptionHandling();
            //ResourceLoader.NotDisposingResources = true;
            //ResourceLoader.CacheImagesAndMasks = false;

            GameEnum.Init();
#endif      
            var client = new MonogameClient(new MonogameClientSettings()
            {
                OriginWidthPixel=1600,
                OriginHeightPixel=900,
                WidthPixel = 1600,// (int)Math.Round(1600/1.5),
                HeightPixel = 900,//(int)Math.Round(900/1.5),
                IsFullScreen = false,
                Add2DLighting = false
            });
            DungeonGlobal.ClientRun = client.Run;
            DungeonGlobal.Run();
        }
    }
}