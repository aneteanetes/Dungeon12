namespace Dungeon
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


        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public static Type[] AllAssignedFrom(this Type type)
        {
            if (!___AllCache.TryGetValue(type, out var value))
            {
                value = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                    .Where(t => type.IsAssignableFrom(t))
                    .ToArray();
                ___AllCache.Add(type, value);
            }

            return value;
        }
        private static readonly Dictionary<Type, Type[]> ___AllCache = new Dictionary<Type, Type[]>();


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

        public static object GetPropertyOfStaticClass(this string staticClass, string property, string assemblyName = default)
        {
            if (assemblyName == default)
            {
                assemblyName = staticClass.Split(".").Last();
            }

            return GetTypeFromAssembly(staticClass, assemblyName).GetStaticProperty(property);
        }

        public static T GetPropertyOfStaticClass<T>(this string staticClass, string property, string assemblyName = default)
        {
            if (assemblyName == default)
            {
                assemblyName = staticClass.Split(".").Last();
            }

            var value = GetTypeFromAssembly(staticClass, assemblyName).GetStaticProperty(property);
            if (value is T valueT)
            {
                return valueT;
            }

            throw new Exception("Property of wrong type!");
        }

        public static Type GetTypeFromAssembly(this string value, string assemblyName, Func<string, Assembly, Type> searchPattern)
        {
            if (!LoadedAssemblies.TryGetValue(assemblyName, out var assembly))
            {
                assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{assemblyName}.dll"));
                LoadedAssemblies.Add(assemblyName, assembly);
            }

            return searchPattern(value, assembly);
        }

        public static Assembly GetAssembly(this string assemblyName)
        {
            if (!LoadedAssemblies.TryGetValue(assemblyName, out var assembly))
            {
                assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{assemblyName}.dll"));
                LoadedAssemblies.Add(assemblyName, assembly);
            }

            return assembly;
        }

        public static Type GetTypeFromAssembly(this string value, string assemblyName)
            => GetTypeFromAssembly(value, assemblyName, (x, a) => a.GetType(x));

        public static T GetInstanceFromAssembly<T>(this string value, string assemblyName, params object[] arguments)
            => GetTypeFromAssembly(value, assemblyName, (x, a) => a.GetType(value) ?? a.GetTypes().FirstOrDefault(t => t.Name == x))
                .NewAs<T>(arguments);

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

        public static object GetType(string typeName)
        {
            Type type = null;
            foreach (var asm in DungeonGlobal.Assemblies)
            {
                type = asm?.GetTypes().FirstOrDefault(x => x.Name == typeName);
                if (type != default)
                {
                    break;
                }
            }

            return type?.New();
        }
    }
}