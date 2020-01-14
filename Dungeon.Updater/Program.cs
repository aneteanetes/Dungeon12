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
            var command = args?.ElementAtOrDefault(0);

            if (command == "pack")
            {
                Pack.Run(args[1], args[2], args[3]);
            }
            else if (command == "unpack")
            {
                Unpack.Run(args[1]);
            }
        }
    }
}