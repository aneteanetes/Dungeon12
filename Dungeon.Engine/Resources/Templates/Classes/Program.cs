using Dungeon.Resources;
using System;

namespace {Project}
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            DungeonGlobal.BindGlobal<Global>();
            DungeonGlobal.ExceptionRethrow = {ExceptionRethrow};
            {GlobalExceptionHandling}DungeonGlobal.GlobalExceptionHandling();
            ResourceLoader.NotDisposingResources = {NotDisposingResources}; //default false
            ResourceLoader.CacheImagesAndMasks = {CacheImagesAndMasks};

            var resCompiler = new ResourceCompiler();
            resCompiler.PreCompiled = true;
            resCompiler.PreCompiledPath={DataPath};
            resCompiler.Compile();

            DungeonGlobal.ClientRun = MonogameClient.Run;
            DungeonGlobal.Run();
        }
    }
}