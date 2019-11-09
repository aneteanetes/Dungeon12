using Dungeon.Data;
using System;
using System.Diagnostics;

namespace Dungeon.Monogame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
#if COMPILEDATABASE
            Database.Init();
#endif
            Database.LoadAllAssemblies();

            try
            {
                using (var game = new XNADrawClient())
                {
                    game.Run();
                }
            }
            catch (ExecutionEngineException ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex; 
            }

            Console.ReadLine();
        }
    }
}