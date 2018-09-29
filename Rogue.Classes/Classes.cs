using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rogue.Entites.Alive.Character;
using System.Runtime.Loader;

namespace Rogue
{
    public static class Classes
    {
        private static bool ClassesLoaded = false;
        private static IEnumerable<Player> classes;
        public static IEnumerable<Player> All()
        {
            if (classes == null)
            {
                CheckAssemblyLoaded();

                   var classTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x=>!x.IsDynamic)
                    .SelectMany(x => x.GetTypes()
                        .Where(t=>!t.IsAbstract)
                        .Where(t => typeof(Player).IsAssignableFrom(t)));

                classes = classTypes
                    .Select(x => (Player)x.New())
                    .Where(x => !string.IsNullOrEmpty(x.ClassName));
            }

            return classes;
        }

        private static void CheckAssemblyLoaded()
        {
            if (!ClassesLoaded)
            {
                ClassesLoaded = true;

                var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Rogue.Classes.*.dll");
                foreach (var file in files)
                {
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                }
            }
        }
    }
}
