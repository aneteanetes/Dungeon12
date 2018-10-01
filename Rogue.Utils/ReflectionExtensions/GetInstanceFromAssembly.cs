namespace Rogue.Utils.ReflectionExtensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Loader;

    public static class GetInstanceFromAssembly
    {
        private static readonly Dictionary<string, Assembly> LoadedAssemblies = new Dictionary<string, Assembly>();        

        public static Type GetTypeFromAssembly(this string value, string assemblyName, Func<string, Assembly, Type> searchPattern)
        {
            if (!LoadedAssemblies.TryGetValue(assemblyName, out var assembly))
            {
                assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{assemblyName}.dll"));
                LoadedAssemblies.Add(assemblyName, assembly);
            }

            return searchPattern(value, assembly);
        }

        public static object GetInstance(this string value, string assemblyName, Func<string, Assembly, Type> searchPattern)
        {
            var type = value.GetTypeFromAssembly(assemblyName, searchPattern);

            return type.New();
        }
    }
}