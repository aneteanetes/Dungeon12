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
            Console.WriteLine(Global.Version);
#if DEBUG
            Global.ExceptionRethrow = true;
            //Global.GlobalExceptionHandling();
            ResourceLoader.NotDisposingResources = true;
            ResourceLoader.CacheImagesAndMasks = false;
            Database.Init();
#endif      
            Database.LoadAllAssemblies();

            Run();
        }

        static void Run(bool FATAL=false)
        {
            //try
            //{
                using (var game = new XNADrawClient())
                {
                    game.isFatal = FATAL;
                    Global.Exit += () =>
                    {
                        game.Dispose();
                        game.Exit();
                        Environment.Exit(0);
                    };
                    game.Run();
                }
            //}
            //catch (Exception ex)
            //{

            //    Global.Logger.Log(ex.ToString());
            //    Run(true);
            //}
        }
    }
}