using Dungeon;
using Dungeon.Monogame;
using Dungeon.Resources;
using System;
using System.Diagnostics;

namespace Dungeon12
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
            Store.Init(Global.GetSaveSerializeSettings());
#endif      

            var monogameClient = new MonogameClient(new MonogameClientSettings()
            {
                IsFullScreen=false,
                WidthPixel=1280,
                HeightPixel=720,
            });

            DungeonGlobal.ClientRun = monogameClient.Run;

            DungeonGlobal.Run();
        }
    }
}
