using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;

namespace Dungeon.Updater
{
    public static class Unpack
    {
        public static void Run(string path)
        {
            for (int i = 0; i < 10; i++)
            {
                if (CheckGameIsRunning())
                {
                    Thread.Sleep(1000);
                }
            }

            if (CheckGameIsRunning())
            {
                Environment.Exit(-1);
            }

            ZipFile.ExtractToDirectory($"{path}", "./", true);

            var game = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dungeon.Monogame.exe");

            using (var process = new Process())
            {
                process.StartInfo.FileName = game;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();


                string a = "";

                while (!process.StandardOutput.EndOfStream)
                {
                    var line = process.StandardOutput.ReadLine();
                    a += Environment.NewLine + line;
                }


                File.WriteAllText("D:\\updaterlog.txt", a);

                Environment.Exit(0);
            }
        }

        private static bool CheckGameIsRunning()
        {
            Process[] pname = Process.GetProcessesByName("Dungeon.Monogame");
            return pname.Length != 0;

        }
    }
}
