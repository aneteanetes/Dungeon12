using Dungeon;
using Dungeon.Monogame;
using Dungeon.Resources;
using System;

namespace InTheWood
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
#endif      
            var client = new MonogameClient(new MonogameClientSettings()
            {
                WidthPixel = 1280,
                HeightPixel = 720,
                IsFullScreen = false,
                Add2DLighting = false,
                CellSize=1
            });
            DungeonGlobal.ClientRun = client.Run;
            DungeonGlobal.Run();
        }
    }
}
