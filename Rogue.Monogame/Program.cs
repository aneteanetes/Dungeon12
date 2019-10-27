using System;
using System.Diagnostics;

namespace Rogue.Monogame
{
    public static class Program
    {
        private static bool CompileDatabase => false;

        [STAThread]
        static void Main()
        {
            if (CompileDatabase)
                Rogue.DataAccess.Program.Main(new string[0]);

            using (var game = new XNADrawClient())
                game.Run();
        }
    }
}