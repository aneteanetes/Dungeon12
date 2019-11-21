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