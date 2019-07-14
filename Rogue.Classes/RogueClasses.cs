namespace Rogue
{
    using Rogue.Entites.Alive;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Loader;

    public static class RogueClasses
    {
        private static bool ClassesLoaded = false;
        private static IEnumerable<Character> classes;
        public static IEnumerable<Character> All()
        {
            if (classes == null)
            {
                CheckAssemblyLoaded();

                   var classTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x=>!x.IsDynamic)
                    .SelectMany(x => x.GetTypes()
                        .Where(t=>!t.IsAbstract)
                        .Where(t => typeof(Character).IsAssignableFrom(t)));

                classes = classTypes
                    .Select(x => (Character)x.New())
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