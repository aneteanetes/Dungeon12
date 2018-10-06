using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Rogue.Resources
{
    public static class ResourceLoader
    {
        public static Stream Load(string resource)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = resource;

            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            return stream;
        }

        public static Stream Load(string resource, string assemblyName)
        {
            var assembly = GetAssembly(assemblyName);
            var resourceName = resource;

            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            return stream;
        }

        private static Assembly GetAssembly(string pathInAssembly)
        {
            var assemblyParts = new List<string>(Path.GetFileNameWithoutExtension(pathInAssembly).Split("."));
            
            while (assemblyParts.Count > 0)
            {
                var assemblyPath = string.Join('.', assemblyParts);

                try
                {
                    var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{assemblyPath}.dll"));
                    return asm;
                }
                catch
                {
                    assemblyParts.RemoveAt(assemblyParts.Count - 1);
                }
            }

            return null;
        }
    }
}
