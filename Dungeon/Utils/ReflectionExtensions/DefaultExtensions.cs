using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Dungeon.Utils.ReflectionExtensions
{
    public static class DefaultExtensions
    {
        /// <summary>
        /// Provide default(Type)
        /// </summary>
        /// <param name="type"></param>
        /// <returns>default(Type)</returns>
        public static object GetDefault(this Type type) => LabelExpression.Lambda(Expression.Default(type)).Compile().DynamicInvoke();
    }
}
