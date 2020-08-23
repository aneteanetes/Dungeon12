using Dungeon;
using Dungeon.Monogame;
using Dungeon.Resources;
using System;

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
            DungeonGlobal.ClientRun = MonogameClient.Run;
            DungeonGlobal.Run();
        }
    }
}
