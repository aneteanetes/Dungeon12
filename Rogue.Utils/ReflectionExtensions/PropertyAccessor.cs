namespace Rogue
{
    using FastMember;
    using System.Linq.Expressions;
    using System;
    using System.Linq;

    public static class PropertyAccessor
    {
        public static T As<T>(this object obj)
        {
            if (obj is T tObj)
            {
                return tObj;
            }

            if(obj is null)
            {
                throw new System.ArgumentNullException("Property is null!");
            }

            throw new System.Exception("Property had wrong type!");
        }

        public static void Dispatch<T>(this T obj, Expression<Action<T, object>> method, object arg)
        {
            var name = (method.Body as MethodCallExpression).Method.Name.Replace("Call", "");

            if (DispatchExists(obj.GetType(), name, arg.GetType()))
            {
                method.Compile().Invoke((T)obj, arg);
            }
        }

        public static TResult Dispatch<T, TResult, TArg>(this T obj, Expression<Func<T, TArg, TResult>> method, TArg arg)
        {
            var name = ((method.Body as UnaryExpression).Operand as MethodCallExpression).Method.Name.Replace("Call", "");

            if (DispatchExists<T>(name,typeof(TArg)))
            {
                return method.Compile().Invoke(obj, arg);
            }

            return default;
        }

        //public static void Dispatch<T, TArg>(this T obj, Expression<Action<T, TArg>> method, TArg arg)
        //{
        //    var name = ((method.Body as UnaryExpression).Operand as MethodCallExpression).Method.Name;

        //    if (DispatchExists<T>(name, typeof(TArg)))
        //    {
        //        method.Compile().Invoke(obj, arg);
        //    }
        //}

        private static bool DispatchExists<TType>(string methodName, Type argType)=> DispatchExists(typeof(TType), methodName, argType);

        private static bool DispatchExists(Type type, string methodName, Type argType)
        {
            return type.GetMethods().Any(m => m.Name == methodName && m.GetParameters()[0].ParameterType == argType);
        }

        public static object GetProperty(this object @object, string property)
        {
            var accessor = TypeAccessor.Create(@object.GetType(), true);
            return accessor[@object, property];
        }

        public static TValue GetProperty<TValue>(this object @object, string property)
        {
            var accessor = TypeAccessor.Create(@object.GetType(), true);
            return (TValue)accessor[@object, property];
        }

        public static TObject SetProperty<TObject, TValue>(this TObject @object, string property, TValue value)
        {
            var accessor = TypeAccessor.Create(@object.GetType(), true);
            accessor[@object, property] = value;
            return @object;
        }
    }
}