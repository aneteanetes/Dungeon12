namespace Dungeon.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using Dungeon.Resources;
    using Dungeon.Types;
    using LiteDB;

    public static partial class Database
    {
        private static string MainPath = $@"{AppDomain.CurrentDomain.BaseDirectory}";

        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public static IEnumerable<T> Entity<T>(Expression<Func<T, bool>> predicate = null, object cacheObject = default)
        {
            if (cacheObject == default)
                return EntityQuery<T>(predicate);

            var key = new CompositeTypeKey<object>()
            {
                Owner = typeof(T),
                Value = predicate ?? cacheObject
            };

            if (!___EntityCache.TryGetValue(key, out var value))
            {
                value = EntityQuery(predicate).ToArray();
                ___EntityCache.Add(key, value);
            }

            return value.As<IEnumerable<T>>();
        }

        private static readonly Dictionary<CompositeTypeKey<object>, object> ___EntityCache = new Dictionary<CompositeTypeKey<object>, object>();
        
        /// <summary>
        /// это надо кэшировать
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T Entity<T>(string type, string id)
        {
            //GetEntityTypeLambdaRuntime(type)

            var t = ResourceLoader.LoadType(type);

            var funcType = typeof(Func<,>).MakeGenericType(new Type[] { t, typeof(bool) });

            var boolExprParam = Expression.Parameter(typeof(bool));

            var typeParameter = Expression.Parameter(t);

            var propExpr = Expression.Property(typeParameter, "IdentifyName");

            var idExpressionParam = Expression.Parameter(typeof(string));

            var setExpr = Expression.Equal(propExpr, Expression.Constant(id));

            var exprParam = Expression.Lambda(funcType, setExpr, typeParameter);

            var expressionTypeParam = Expression.Parameter(exprParam.GetType());
            var objParam = Expression.Parameter(typeof(object));

            var genericEntity = typeof(Database).GetMethods().FirstOrDefault(x => x.Name == "Entity" && x.GetParameters().FirstOrDefault().ParameterType != typeof(string));
            genericEntity = genericEntity.MakeGenericMethod(t);
            
            var methodCall = Expression.Call(genericEntity, expressionTypeParam,objParam);

            var entity = Expression.Lambda(methodCall, expressionTypeParam, objParam).Compile().DynamicInvoke(exprParam, default);
            var @enum = entity.As<IEnumerable>().GetEnumerator();
            @enum.MoveNext();
            return @enum.Current.As<T>();
        }


        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        private static Delegate GetEntityTypeLambdaRuntime(string typeName)
        {
            if (!___GetEntityTypeLambdaRuntimeCache.TryGetValue(typeName, out var value))
            {
                value = //логика кэширования
                  default;
                ___GetEntityTypeLambdaRuntimeCache.Add(typeName, value);
            }

            return value;
        }
        private static readonly Dictionary<string, Delegate> ___GetEntityTypeLambdaRuntimeCache = new Dictionary<string, Delegate>();


        public static IEnumerable<T> EntityQuery<T>(Expression<Func<T, bool>> predicate = null)
        {
            if (!Directory.Exists(MainPath))
                Directory.CreateDirectory(MainPath);

            using (var db = new LiteDatabase($@"{MainPath}\Data.db"))
            {
                var collection = db.GetCollection<T>();
                
                if (predicate != null)
                    return collection.Find(predicate);

                return collection.FindAll();
            }
        }
    }
}