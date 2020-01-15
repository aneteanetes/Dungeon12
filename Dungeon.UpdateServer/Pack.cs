using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Dungeon.UpdateServer
{
    public static class Pack
    {
        public static void Run(string fromVersionFolder, string nextVersionFolder, string name, string destination, string temp)
        {
            var prev = FilesHash(fromVersionFolder);
            var next = FilesHash(nextVersionFolder);

            var diff = next.Where(x =>
            {
                var isNewFile = prev.FirstOrDefault(p => p.Path == x.Path);
                if (isNewFile == default)
                {
                    return true;
                }
                else
                {
                    return !File.ReadAllBytes(isNewFile.FullPath).SequenceEqual(File.ReadAllBytes(x.FullPath));
                }
            });

            var update = Directory.CreateDirectory(temp);
            foreach (var diffFile in diff)
            {
                File.Copy(diffFile.FullPath, Path.Combine(update.FullName, Path.GetFileName(diffFile.FullPath)));
            }

            ZipFile.CreateFromDirectory(update.FullName, destination);
        }

        private static List<GameFile> FilesHash(string path)
        {
            var files = new List<GameFile>();
            foreach (var file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
            {
                files.Add(new GameFile()
                {
                    FullPath = file,
                    Path = file.Replace(path, "")
                });
            }

            return files;
        }

        private class GameFile
        {
            public string FullPath { get; set; }

            public string Path { get; set; }

            public string Hash { get; set; }
        }
    }
}
