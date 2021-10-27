using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Dungeon.Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            var zippath = args?.ElementAtOrDefault(0);

            if (zippath != default)
            {
                try
                {
                    Unpack.Run(args[0]);
                }
                catch (Exception ex)
                {
                    File.WriteAllText("updaterlog.txt", ex.ToString());
                    throw;
                }
            }
        }
    }
}