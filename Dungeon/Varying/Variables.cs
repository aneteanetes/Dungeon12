using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon.Varying
{
    public class Variables
    {
        private static Dictionary<string, Variable> variables = new();

        public static T Get<T>(string name, T @default = default)
        {
            var variable = TryGet<T>(name, @default);

            return variable.Get<T>();
        }

        public static void Set<T>(string name, T value)
        {
            var variable = TryGet<T>(name);
            variable.Set(value);
        }

        public static void OnChange<T>(string name, Action action)
        {
            var variable = TryGet<T>(name);
            variable.OnChange += action;
        }

        private static Variable TryGet<T>(string name, T @default = default)
        {
            if (!variables.TryGetValue(name, out var variable))
            {
                variables[name] = variable = new Variable<T>(name);
                variable.Set(@default.ToString());
            }

            return variable;
        }

        public static List<Variable> Values => variables.Select(x => x.Value).ToList();
    }
}
