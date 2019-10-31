using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Rogue
{
    public static class IsDefaultExtension
    {

        /// <summary>
        /// Определяет является ли объект равен default через деревья выражений
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public static bool IsDefault(this object @object)
        {
            var type = @object.GetType();

            if (!___IsDefaultCache.TryGetValue(type, out var value))
            {
                var @default = Expression.Default(type);

                var p = Expression.Parameter(type);

                value = Expression.Lambda(Expression.Equal(p, @default), p).Compile();

                ___IsDefaultCache.Add(type, value);
            }

            return (bool)value.DynamicInvoke(@object);
        }

        private static readonly Dictionary<Type, Delegate> ___IsDefaultCache = new Dictionary<Type, Delegate>();
    }
}
