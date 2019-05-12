namespace Rogue
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Loader;

    public static class GetInstanceFromAssemblyExtensions
    {
        private static readonly Dictionary<string, Assembly> LoadedAssemblies = new Dictionary<string, Assembly>();

        public static T GetInstanceFromAssembly<T>(this object assemblyType)
            => GetTypeFromAssembly<T>(assemblyType)
                .NewAs<T>();

        public static T[] GetInstancesFromAssembly<T>(this object assemblyType)
            => GetTypesFromAssembly<T>(assemblyType)
                .Select(x => x.NewAs<T>())
                .ToArray();

        public static Type[] GetTypesFromAssembly<T>(this object assemblyType)
            => assemblyType.GetType()
                .Assembly
                .GetTypes()
                .Where(x => typeof(T).IsAssignableFrom(x))
                .ToArray();

        public static Type GetTypeFromAssembly<T>(this object assemblyType)
            => assemblyType.GetType()
                .Assembly
                .GetTypes()
                .FirstOrDefault(x => typeof(T).IsAssignableFrom(x));

        public static Type GetTypeFromAssembly(this string value, string assemblyName, Func<string, Assembly, Type> searchPattern)
        {
            if (!LoadedAssemblies.TryGetValue(assemblyName, out var assembly))
            {
                assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{assemblyName}.dll"));
                LoadedAssemblies.Add(assemblyName, assembly);
            }

            return searchPattern(value, assembly);
        }

        public static Type[] GetTypesFromAssembly(this string value, string assemblyName, Func<string, Assembly, IEnumerable<Type>> searchPattern)
        {
            if (!LoadedAssemblies.TryGetValue(assemblyName, out var assembly))
            {
                assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{assemblyName}.dll"));
                LoadedAssemblies.Add(assemblyName, assembly);
            }

            return searchPattern(value, assembly).ToArray();
        }

        public static object GetInstance(this string value, string assemblyName, Func<string, Assembly, Type> searchPattern)
        {
            var type = value.GetTypeFromAssembly(assemblyName, searchPattern);

            return type.New();
        }

        public static T[] GetInstancesFromAssembly<T>(this string value, string assemblyName, Func<string, Assembly, IEnumerable<Type>> searchPattern)
        {
            var types = value.GetTypesFromAssembly(assemblyName, searchPattern);

            return types.Select(x => (T)x.New()).ToArray();
        }
    }
}