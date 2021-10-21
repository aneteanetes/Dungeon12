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
            DungeonGlobal.ComponentUpdateCompatibility = false;
            Console.WriteLine(DungeonGlobal.Version);
#if DEBUG
            var resCompiler = new ResourceCompiler();
            resCompiler.Compile();

            DungeonGlobal.ExceptionRethrow = true;
            DungeonGlobal.GlobalExceptionHandling();
            //ResourceLoader.NotDisposingResources = true;
            //ResourceLoader.CacheImagesAndMasks = false;

#endif      
            GameEnum.Init();
            var client = new MonogameClient(new MonogameClientSettings()
            {
                OriginWidthPixel=1600,
                OriginHeightPixel=900,
                IsFullScreen = false,
                Add2DLighting = false
            });
            DungeonGlobal.ClientRun = client.Run;
            DungeonGlobal.Run();
        }
    }
}