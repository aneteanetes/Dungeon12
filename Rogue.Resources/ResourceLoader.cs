using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
#if Android
using System.Linq;
#endif

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

        private static Dictionary<string, Stream> RuntimeCache = new Dictionary<string, Stream>();
        public static void SaveStream(Stream stream, string image)
        {
            RuntimeCache[image] = stream;
        }

        public static Stream Load(string resource, string assemblyName)
        {
#if Android
            if(assemblyName.Contains("Rogue.Resources.Particles"))
            {
                assemblyName = assemblyName.Replace("Rogue.Resources.Particles", "Rogue.Resources.Android.Particles");
            }
            if (resource.Contains("Rogue.Resources.Particles"))
            {
                resource = resource.Replace("Rogue.Resources.Particles", "Rogue.Resources.Android.Particles");
            }
            if (resource.Contains("Rogue.Resources.")&& !resource.Contains("Rogue.Resources.Android."))
            {
                resource = resource.Replace("Rogue.Resources.", "Rogue.Resources.Android.");
            }
            if (assemblyName.Contains("Rogue.Resources.") && !assemblyName.Contains("Rogue.Resources.Android."))
            {
                assemblyName = assemblyName.Replace("Rogue.Resources.", "Rogue.Resources.Android.");
            }
#endif

            if (RuntimeCache.ContainsKey(resource))
            {
                var ms = RuntimeCache[resource];
                if (ms.CanSeek)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                }
                return ms;
            }

            var assembly = GetAssembly(assemblyName);
            var resourceName = resource;

            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == default)
                return stream;

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
                    Assembly asm = null;
#if Core
                    asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{assemblyPath}.dll"));
#endif
#if Android
                    if(asm==null)
                    {
                        asm= Assembly.Load($"{assemblyPath}");
                    }
                    if(asm==null)
                    {
                        asm = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == assemblyPath);
                    }
#endif
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
