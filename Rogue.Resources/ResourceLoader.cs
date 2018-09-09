using System;
using System.IO;
using System.Reflection;

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
    }
}
