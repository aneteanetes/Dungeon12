namespace Dungeon
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using Dungeon.Data;
    using Dungeon.Resources;
    using Dungeon.Types;
    using LiteDB;
    using Newtonsoft.Json;

    public static partial class Store
    {
        public static string MainPath = $@"{AppDomain.CurrentDomain.BaseDirectory}";

        public static T EntitySingle<T>(string id, object cacheObject = default, string db = "Data")
            where T : IPersist
        {
            return Entity<T>(x => x.IdentifyName == id, cacheObject, db).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public static IEnumerable<T> Entity<T>(Expression<Func<T, bool>> predicate = null, object cacheObject = default,string db="Data")
            where T : IPersist
        {
            if (cacheObject == default)
                return EntityQuery<T>(predicate,db);

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
        public static T EntitySingle<T>(string type, string id)
            where T : IPersist
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

            var dbParam = Expression.Parameter(typeof(string));

            var genericEntity = typeof(Store).GetMethods().FirstOrDefault(x => x.Name == "Entity" && x.GetParameters().FirstOrDefault().ParameterType != typeof(string));
            genericEntity = genericEntity.MakeGenericMethod(t);

            var methodCall = Expression.Call(genericEntity, expressionTypeParam, objParam, dbParam);

            var entity = Expression.Lambda(methodCall, expressionTypeParam, objParam, dbParam).Compile().DynamicInvoke(exprParam, default,"Data");
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


        public static IEnumerable<T> EntityQuery<T>(Expression<Func<T, bool>> predicate = null, string dbName = "Data")
        {
            if (!Directory.Exists(MainPath))
                Directory.CreateDirectory(MainPath);

            using (var db = new LiteDatabase($@"{MainPath}\{dbName}.db"))
            {
                var collection = db.GetCollection<T>();

                if (predicate != null)
                    return collection.Find(predicate);

                return collection.FindAll();
            }
        }
    }
}