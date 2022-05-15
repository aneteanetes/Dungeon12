using Dungeon;
using Dungeon.Monogame;
using Dungeon.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dungeon12
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            DungeonGlobal.BindGlobal<Global>(true,true);
#if DEBUG
            ResourceLoader.Settings.EmbeddedMode = false;


            //DungeonGlobal.ExceptionRethrow = true;
            //DungeonGlobal.GlobalExceptionHandling();
            //ResourceLoader.NotDisposingResources = true;
            //ResourceLoader.CacheImagesAndMasks = false;

#endif      
            ResourceLoader.ResourceResolvers.Add(new EmbeddedResourceResolver(Assembly.GetExecutingAssembly()));

            var width = 1920;
            var height = 1080;
            var monitor = 1;

            if (args!=null && args.Length > 0)
            {
                int.TryParse(args.ElementAtOrDefault(0) ?? "1920", out width);
                int.TryParse(args.ElementAtOrDefault(1) ?? "1080", out height);
                int.TryParse(args.ElementAtOrDefault(2) ?? "1", out monitor);
            }

            var client = new MonogameClient(new MonogameClientSettings()
            {
                OriginWidthPixel = width,
                OriginHeightPixel = height,
                IsFullScreen = true,
                Add2DLighting = false,
                IsWindowedFullScreen = true,
                MonitorIndex = monitor
            });
            DungeonGlobal.ClientRun = client.Run;
            DungeonGlobal.Run();
        }
    }
}