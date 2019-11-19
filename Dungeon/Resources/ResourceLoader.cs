using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Linq;

namespace Dungeon.Resources
{
    public static class ResourceLoader
    {
        public static Stream Load(string resource)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = resource;

            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == default)
            {
                stream = Global.GameAssembly.GetManifestResourceStream(resourceName);
            }

            if (stream == default)
            {
                foreach (var asm in Global.Assemblies)
                {
                    stream = asm.GetManifestResourceStream(resourceName);
                    if (stream != default)
                    {
                        break;
                    }
                }
            }

            if (stream == default)
            {
                throw new KeyNotFoundException($"Ресурс {resource} не найден!");
            }

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
            if(assemblyName.Contains("Dungeon.Resources.Particles"))
            {
                assemblyName = assemblyName.Replace("Dungeon.Resources.Particles", "Dungeon.Resources.Android.Particles");
            }
            if (resource.Contains("Dungeon.Resources.Particles"))
            {
                resource = resource.Replace("Dungeon.Resources.Particles", "Dungeon.Resources.Android.Particles");
            }
            if (resource.Contains("Dungeon.Resources.")&& !resource.Contains("Dungeon.Resources.Android."))
            {
                resource = resource.Replace("Dungeon.Resources.", "Dungeon.Resources.Android.");
            }
            if (assemblyName.Contains("Dungeon.Resources.") && !assemblyName.Contains("Dungeon.Resources.Android."))
            {
                assemblyName = assemblyName.Replace("Dungeon.Resources.", "Dungeon.Resources.Android.");
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

        public static Type LoadType(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
                return default;

            var type = TryGetFromAssembly(className, Global.GameAssembly);
            if (type == default)
            {
                foreach (var asm in Global.Assemblies)
                {
                    type = TryGetFromAssembly(className, asm);
                    if (type != default)
                    {
                        break;
                    }
                }
            }

            if (type == default)
            {
                throw new DllNotFoundException($"Тип {className} не найден ни в одной из загруженных сборок!");
            }

            return type;
        }

        private static Type TryGetFromAssembly(string className, Assembly assembly)
        {
            var type = assembly.GetType(className);
            if (type == default)
            {
                type = assembly.GetTypes().FirstOrDefault(x => x.Name == className);
            }

            return type;
        }
    }
}