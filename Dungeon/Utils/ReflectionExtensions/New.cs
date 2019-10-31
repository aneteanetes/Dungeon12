namespace Dungeon
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class ReflectionExtensions
    {
        /// <summary>
        /// Создать новый объект такого типа
        /// <para>
        /// Только вот не надо использовать это на типах <see cref="Type"/>
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <returns></returns>
        public static T New<T>(this object @object) => NewAs<T>(@object.GetType());

        /// <summary>
        /// Instantiate new object through expression tree with first ctor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="argsObj"></param>
        /// <returns></returns>
        public static T New<T>(this Type type, params object[] argsObj)
            => New<T>(type, typeof(T).GetConstructors().FirstOrDefault(), argsObj);

        /// <summary> 
        /// Instantiate new object through expression tree with first ctor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="argsObj"></param>
        /// <returns></returns>
        public static object New(this Type type, params object[] argsObj)
            => New<object>(type, type.GetConstructors().FirstOrDefault(), argsObj);

        /// <summary>
        /// Instantiate new object through expression tree
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="ctor"></param>
        /// <param name="argsObj"></param>
        /// <returns></returns>
        public static T New<T>(this Type type, ConstructorInfo ctor, params object[] argsObj)
        {
            ParameterInfo[] par = ctor.GetParameters();
            Expression[] args = new Expression[par.Length];
            ParameterExpression param = Expression.Parameter(typeof(object[]));
            for (int i = 0; i != par.Length; ++i)
            {
                args[i] = Expression.Convert(Expression.ArrayIndex(param, Expression.Constant(i)), par[i].ParameterType);
            }
            var expression = Expression.Lambda<Func<object[], T>>(
                Expression.New(ctor, args), param
            );

            var func = expression.Compile();

            return func(argsObj);
        }

        /// <summary>
        /// Инстанциирует объект как object, а затем приводит к T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="argsObj"></param>
        /// <returns></returns>
        public static T NewAs<T>(this Type type, params object[] argsObj)
            => (T)New<object>(type, type.GetConstructors().FirstOrDefault(), argsObj);

        public static void Call(this object @object, string method, params object[] argsObj)
        {
            var methodInfo = @object.GetType().GetMethods().FirstOrDefault(m => m.Name == method);
            if (methodInfo != default)
            {
                Expression.Lambda(Expression.Call(Expression.Constant(@object), methodInfo)).Compile().DynamicInvoke();
            }
        }

        public static void Call(this object obj, Type generics, params object[] argsObj)
        {

        }
    }
}