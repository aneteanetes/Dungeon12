using Dungeon.Data;
using Dungeon.Resources;
using System;
using System.Diagnostics;

namespace Dungeon.Monogame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
#if DEBUG
            ResourceLoader.NotDisposingResources = true;
            ResourceLoader.CacheImagesAndMasks = true;
            Database.Init();
#endif      
            Database.LoadAllAssemblies();

            using (var game = new XNADrawClient())
            {
                Global.Exit += () =>
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