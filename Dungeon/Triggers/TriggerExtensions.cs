using Dungeon.Data;
using Dungeon.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dungeon
{
    public static class TriggerExtensions
    {
        /// <summary>
        /// Создаёт триггер из имени типа
        /// </summary>
        /// <typeparam name="TTrigger"></typeparam>
        /// <param name="className"></param>
        /// <returns></returns>
        public static TTrigger Trigger<TTrigger>(this string className)
            where TTrigger : class, ITrigger
        {
            if (string.IsNullOrWhiteSpace(className))
                return default;

            var type = TryGetFromAssembly(className, DungeonGlobal.GameAssembly);
            if (type == default)
            {
                foreach (var asm in DungeonGlobal.Assemblies)
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
                throw new DllNotFoundException($"Тип триггера {className} не найден ни в одной из загруженных сборок!");
            }

            var trigger = type.NewAs<TTrigger>();
            if (trigger == default)
            {
                throw new TypeLoadException($"Тип триггера {className} инстанциирован не верно!");
            }

            return trigger;
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

        public static ITrigger Trigger(this string className)
        {
            var triggerType = ResourceLoader.LoadType(className);
            return new RuntimeTrigger(triggerType);
        }

        private class RuntimeTrigger : ITrigger
        {
            private object _trigger;

            public RuntimeTrigger(Type type)
            {
                _trigger = type.New();
            }

            public void Unkown(params object[] args)
            {
                _trigger.Call("Trigger", args);
            }
        }
    }
}
