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

            using (var game = new XNADrawClient())
                game.Run();
        }
    }
}