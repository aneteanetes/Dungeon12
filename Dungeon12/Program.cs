using Dungeon;
using Dungeon.Monogame;
using Dungeon.Resources;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Dungeon12
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
            ResourceLoader.Settings.EmbeddedMode = true;

            //var resCompiler = new ResourceCompiler();
            //resCompiler.Compile();

            //DungeonGlobal.ExceptionRethrow = true;
            //DungeonGlobal.GlobalExceptionHandling();
            //ResourceLoader.NotDisposingResources = true;
            //ResourceLoader.CacheImagesAndMasks = false;

#endif      
            ResourceLoader.ResourceResolvers.Add(new EmbeddedResourceResolver(Assembly.GetExecutingAssembly()));

            var client = new MonogameClient(new MonogameClientSettings()
            {
                OriginWidthPixel = 1920,
                OriginHeightPixel = 1080,
                IsFullScreen = true,
                Add2DLighting = false,
                IsWindowedFullScreen = true,
                MonitorIndex = 1
            });
            DungeonGlobal.ClientRun = client.Run;
            DungeonGlobal.Run();
        }
    }
}