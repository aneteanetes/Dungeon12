using Dungeon.Data;
using System;
using System.Diagnostics;

namespace Dungeon.Monogame
{
    public static class Program
    {
        private static bool CompileDatabase => true;

        [STAThread]
        static void Main()
        {
            if (CompileDatabase)
            {
                Database.Init();
            }

            using (var game = new XNADrawClient())
                game.Run();
        }
    }
}