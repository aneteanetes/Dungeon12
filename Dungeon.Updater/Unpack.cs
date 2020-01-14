using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Dungeon.Updater
{
    public static class Unpack
    {
        public static void Run(string path)
        {
            ZipFile.ExtractToDirectory($"{path}", "./", true);
            Console.WriteLine("Обновление завершено, нажмите любую клавишу для продолжения...");
            Console.ReadLine();

            var game = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dungeon.Monogame.exe");            

            using (var process = new Process())
            {
                process.StartInfo.FileName = game;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                Environment.Exit(0);
            }
        }
    }
}
