using Dungeon;
using Dungeon.Resources;
using Dungeon12;
using System;

namespace VegaPQ
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            DungeonGlobal.BindGlobal<VegaGlobal>();
#if DEBUG
            var resCompiler = new ResourceCompiler();
            resCompiler.Compile();

            Global.ExceptionRethrow = true;
            Global.GlobalExceptionHandling();
            Store.Init(Global.GetSaveSerializeSettings());
#endif      
            Store.LoadAllAssemblies();

            Run();
        }

        static void Run(bool FATAL = false)
        {
            try
            {
                using (var game = new VegaMG())
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
            }
            catch (Exception ex)
            {
                throw;
                //Global.Logger.Log(ex.ToString());
                Run(true);
            }
        }
    }
}
