using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dungeon.Engine.Utils
{
    public class Container
    {
        public ServiceCollection ServiceCollection { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public Container() => Reset();

        public void Reset()
        {
            ServiceCollection = new ServiceCollection();
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public void Register(ContainerLifeStyle containerLifeStyle, Type typeAs, Type typeImpl)
        {
            switch (containerLifeStyle)
            {
                case ContainerLifeStyle.Transient:
                    ServiceCollection.AddTransient(typeAs, typeImpl);
                    break;
                case ContainerLifeStyle.Singleton:
                    ServiceCollection.AddSingleton(typeAs, typeImpl);
                    break;
                case ContainerLifeStyle.Scope:
                    ServiceCollection.AddScoped(typeAs, typeImpl);
                    break;
                default:
                    break;
            }
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

            public void Register<T, TImpl>(ContainerLifeStyle containerLifeStyle, TImpl implSingleton = default)
            where T : class
            where TImpl : class, T
        {
            switch (containerLifeStyle)
            {
                case ContainerLifeStyle.Transient:
                        ServiceCollection.AddTransient<T, TImpl>();
                    break;
                case ContainerLifeStyle.Singleton:
                    if (implSingleton != default)
                    {
                        ServiceCollection.AddSingleton<T, TImpl>(x => implSingleton);
                    }
                    else
                    {
                        ServiceCollection.AddSingleton<T, TImpl>();
                    }
                    break;
                case ContainerLifeStyle.Scope:
                        ServiceCollection.AddScoped<T, TImpl>();
                    break;
                default:
                    break;
            }
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public T Resolve<T>() => ServiceProvider.GetService<T>();

        public IEnumerable<T> ResolveAll<T>() => ResolveAllImpl<T>();

        private IEnumerable<T> ResolveAllImpl<T>(bool force=true)
        {
            var services = ServiceProvider.GetServices<T>();
            if (services.IsNotEmpty())
            {
                return services;
            }

            if (force)
            {
                var impls = new List<Type>();

                foreach (var asm in DungeonGlobal.Assemblies)
                {
                    try
                    {
                        impls.AddRange(asm.GetTypes().Where(t => typeof(T).IsAssignableFrom(t) & typeof(T)!=t));
                    }
                    catch (ReflectionTypeLoadException)
                    {
                        continue;
                    }
                }

                foreach (var type in impls)
                {
                    Register(ContainerLifeStyle.Transient, typeof(T), type);
                }

                return ResolveAllImpl<T>(false);
            }

            return default;
        }
    }

    public enum ContainerLifeStyle
    {
        Transient = 0,
        Singleton = 1,
        Scope = 2
    }
}
