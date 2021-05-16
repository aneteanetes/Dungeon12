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
                WidthPixel = 1600,
                HeightPixel = 900,
                IsFullScreen = false,                
                Add2DLighting = false
            });
            DungeonGlobal.ClientRun = client.Run;
            DungeonGlobal.Run();
        }
    }
}