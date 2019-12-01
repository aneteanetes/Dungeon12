namespace Dungeon
{
    using FastMember;
    using System.Linq.Expressions;
    using System;
    using System.Linq;
    using Dungeon.Types;
    using System.Reflection;
    using System.Collections.Generic;

    public static class PropertyAccessor
    {
        public static T As<T>(this object obj)
        {
            if (obj == default)
            {
                return default;
            }

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

        public static bool Is<T>(this object obj)
        {
            if (obj is T )
            {
                return true;
            }
            return false;
        }

        public static bool IsNot<T>(this object obj) => !(obj is T);

        public static Func<TClass, TProperty> GetFieldAccessor<TClass, TProperty>(string fieldName)
        {
            ParameterExpression param = Expression.Parameter(typeof(TClass), "arg");

            MemberExpression member = Expression.Field(param, fieldName);

            LambdaExpression lambda = Expression.Lambda(typeof(Func<TClass, TProperty>), member, param);

            Func<TClass, TProperty> compiled = (Func<TClass, TProperty>)lambda.Compile();
            return compiled;
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
            string name = ExtractMethodName(method);

            if (DispatchExists<T>(name, typeof(TArg)))
            {
                return method.Compile().Invoke(obj, arg);
            }

            return default;
        }

        private static string ExtractMethodName(LambdaExpression method)
        {
            return (method.Body as MethodCallExpression).Method.Name;
        }

        private static void Flow<T>(this T obj, Expression<Action<T>> method, object args=null, bool up = true)
            where T : IFlowable
        {
            var caller = up ? UpperFlowable(obj) : obj;

            var methodName = ExtractMethodName(method);
            object ctx = CreateContextObject(caller, methodName);

            if (args != null)
            {
                MergeObjects(ctx, args);
            }

            var firstFlow = DoFlow(ctx, methodName, caller);
            var flow = new List<Action<bool>>()
            {
                firstFlow
            };

            MakeFlow(caller, ctx, methodName, flow);

            flow.Reverse();

            foreach (var flowEntry in flow)
            {
                flowEntry?.Invoke(false);
            }
        }

        private static object CreateContextObject(IFlowable caller, string methodName)
        {
            try
            {
                return (Attribute.GetCustomAttribute(
                        GetFlowMethodInfo(caller, methodName),
                        typeof(FlowMethodAttribute)) as FlowMethodAttribute)
                        .ContextType.New();
            }
            catch (Exception inner)
            {
                throw new Exception("Ошибка при выполнении flow метода - возможно не указан Parent для вызываемого объекта? Либо, у самого верхнего объекта FlowMethodAttribute без аргументов?", inner);
            }
        }

        private static IFlowable UpperFlowable(IFlowable flowable)
        {
            var parent = flowable.GetParentFlow();
            if (parent != null)
            {
                return UpperFlowable(parent);
            }
            return flowable;
        }

        private static void MakeFlow(object obj, object ctx, string methodName, List<Action<bool>> flow)
        {
            if (obj == default)
                return;

            var flowable = FindFlowable(obj);
            foreach (var innerFlow in flowable)
            {
                flow.Add(DoFlow(ctx, methodName, innerFlow));
            }

            foreach (var innerFlow in flowable)
            {
                MakeFlow(innerFlow, ctx, methodName, flow);
            }
        }

        private static Action<bool> DoFlow(object ctx, string methodName, IFlowable innerFlow, bool forward = true)
        {
            innerFlow.SetFlowContext(ctx);
            var flowMethod = CallFlowable(innerFlow, methodName, ctx, forward);
            MergeObjects(ctx, innerFlow.GetFlowContext());

            return flowMethod;
        }

        private static IFlowable[] FindFlowable(object obj)
        {
            var accessor = TypeAccessor.Create(obj.GetType());
            return accessor
                .GetMembers()
                .Where(m => typeof(IFlowable).IsAssignableFrom(m.Type))
                .Select(x => 
                {
                    try
                    {
                        var value = accessor[obj, x.Name];
                        if (value != obj)
                        {
                            return value;
                        }
                        return default;
                    }
                    catch
                    {
                        // потому что fastmember валится когда хочешь Item свойство обработать
                        return default;
                    }
                })
                .Where(x => x != default)
                .Cast<IFlowable>()
                .ToArray();
        }

        private static Action<bool> CallFlowable(object next, string method, object args, bool forward)
        {
            MethodInfo methodInfo = GetFlowMethodInfo(next, method);
            if (methodInfo != null)
            {
                var param = Expression.Parameter(typeof(bool));

                var call = Expression.Call(Expression.Constant(next), methodInfo, param);
                var lambd = Expression.Lambda<Action<bool>>(call, param);

                var flowMethod = lambd.Compile();
                flowMethod.Invoke(forward);
                return flowMethod;
            }

            return default;
        }

        private static MethodInfo GetFlowMethodInfo(object next, string method)
        {
            return next.GetType().GetMethods().FirstOrDefault(x => x.Name == method);
        }

        private static object MergeObjects(object to, object from)
        {
            var accessorTo = TypeAccessor.Create(to.GetType());
            var accessorFrom = TypeAccessor.Create(from.GetType());

            foreach (var prop in accessorTo.GetMembers())
            {
                var propertyForSet = accessorFrom.GetMembers().FirstOrDefault(x => x.Name == prop.Name);
                if (propertyForSet == null)
                    continue;

                try
                {
                    accessorTo[to, prop.Name] = accessorFrom[from, prop.Name];
                }
                catch
                {
                    Console.WriteLine("Попытка установить свойство с неподходящим типом");
                }
            }

            return to;
        }

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

        public static object GetStaticProperty(this Type type,string property)
        {
            return Expression.Lambda(Expression.MakeMemberAccess(null, type.GetMembers().FirstOrDefault(x => x.Name == property))).Compile().DynamicInvoke();
            //var accessor = TypeAccessor.Create(type, true);
            //return accessor[null, property];
        }

        public static TValue GetProperty<TValue>(this object @object, string property, TValue @default=default)
        {
            var accessor = TypeAccessor.Create(@object.GetType(), true);
            try
            {
                return (TValue)accessor[@object, property];
            }
            catch
            {
                return @default;
            }
        }

        public static TValue GetPropertyExpr<TValue>(this object @object, string propName)
        {
            var type = @object.GetType();
            var key = propName + type.AssemblyQualifiedName;

            if (!___GetBackginFieldValueExpressionCache.TryGetValue(key, out var value))
            {
                var p = Expression.Parameter(type);
                value = Expression.Lambda(Expression.PropertyOrField(p, propName), p).Compile();

                ___GetBackginFieldValueExpressionCache.Add(key, value);
            }

            return value.DynamicInvoke(@object).As<TValue>();
        }

        private static readonly Dictionary<string, Delegate> ___GetBackginFieldValueExpressionCache = new Dictionary<string, Delegate>();


        public static TObject SetProperty<TObject, TValue>(this TObject @object, string property, TValue value)
        {
            var accessor = TypeAccessor.Create(@object.GetType(), true);
            accessor[@object, property] = value;
            return @object;
        }

        public static void SetProperty<TValue>(this object @object, string property, TValue value)
        {
            var accessor = TypeAccessor.Create(@object.GetType(), true);
            accessor[@object, property] = value;
        }
    }
}