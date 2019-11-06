using FastMember;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;

namespace Dungeon.Proxy
{
    public class ProxyObject : IDrawable
    {
        protected TypeAccessor _Type;

        public ProxyObject()
        {
            ProxyId = this.GetType().Name;
            InitProxyProperties();
            _Type = TypeAccessor.Create(this.GetType(), true);
        }

        public virtual string ProxyId { get; protected set; }

        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public Func<object> BindGet(string propName, string ownerClassName)
        {
            propName = $"___{propName}";
            if (!___BindGetCache.TryGetValue(propName, out var value))
            {
                value = () => GetBackginFieldValueExpression(ownerClassName, propName);// GetBackingFieldValue(propName, ownerClassName);
                ___BindGetCache.Add(propName, value);
            }

            return value;
        }
        private readonly Dictionary<string, Func<object>> ___BindGetCache = new Dictionary<string, Func<object>>();

        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public Action<object> BindSet<TValue>(string propName, string ownerclassname)
        {
            propName = $"___{propName}";
            if (!___BindSetCache.TryGetValue(propName, out var value))
            {
                value = v => SetBackingFieldValueExpression(v, propName, ownerclassname, typeof(TValue));
                ___BindSetCache.Add(propName, value);
            }

            return value;
        }
        private readonly Dictionary<string, Action<object>> ___BindSetCache = new Dictionary<string, Action<object>>();

        private string PropertyName(string callerProp)
        {
            return callerProp;
        }

        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public ProxiedAttribute Proxies(string prop)
        {
            if (!___ProxiedCache.TryGetValue(prop, out var value))
            {
                value = (ProxiedAttribute)_Type.GetMembers().FirstOrDefault(m => m.Name == prop)?.GetAttribute(typeof(ProxiedAttribute), false);
                ___ProxiedCache.Add(prop, value);
            }

            return value;
        }
        private readonly Dictionary<string, ProxiedAttribute> ___ProxiedCache = new Dictionary<string, ProxiedAttribute>();
        
        private List<ProxyProperty> ProxiesAdditional(string prop)
        {
            if (!Additionals.TryGetValue(prop, out var vals))
            {
                return default;
            }
            return vals;
        }

        private string GetProxyId(string prop) => $"{ProxyId}.{prop}";

        protected TCalculatedType Get<TCalculatedType>(TCalculatedType v, string ownerClassName, [CallerMemberName] string from = "")
        {
            var p = PropertyName(from);
            var get = BindGet(p, ownerClassName);
            var set = BindSet<TCalculatedType>(p, ownerClassName);
            var proxy = Proxies(p);
            var add = ProxiesAdditional(p);
            var proxyId = GetProxyId(p);

            return proxy.Get(v, proxyId, get, set, this, _Type, p, add);
        }

        protected void Set<TCalculatedType>(TCalculatedType v, string ownerClassName, [CallerMemberName] string from = "")
        {
            var p = PropertyName(from);
            var get = BindGet(p, ownerClassName);
            var set = BindSet<TCalculatedType>(p, ownerClassName);
            var proxy = Proxies(p);
            var add = ProxiesAdditional(p);
            var proxyId = GetProxyId(p);

            var backingField = $"___{p}";

            var settedValue = proxy.Set(v, proxyId, get, set, this, _Type, p, add);

            SetBackingFieldValueExpression(settedValue, backingField, ownerClassName, typeof(TCalculatedType));
        }

        private Dictionary<string, FieldInfo> FastMemberCantAccess = new Dictionary<string, FieldInfo>();

        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public object GetBackginFieldValueExpression(string ownerClassName, string propName)
        {
            if (!___GetBackginFieldValueExpressionCache.TryGetValue(propName, out var value))
            {
                value = Expression.Lambda(Expression.Field(Expression.Constant(this), GetField(ownerClassName, propName))).Compile();
                ___GetBackginFieldValueExpressionCache.Add(propName, value);
            }

            return value.DynamicInvoke();
        }
        private readonly Dictionary<string, Delegate> ___GetBackginFieldValueExpressionCache = new Dictionary<string, Delegate>();

        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public FieldInfo GetField(string ownerClassName, string propName)
        {
            var key = ownerClassName + propName;
            if (!___GetFieldCache.TryGetValue(key, out var value))
            {
                value = Type.GetType(ownerClassName).GetField(propName, BindingFlags.Instance | BindingFlags.NonPublic);
                ___GetFieldCache.Add(key, value);
            }

            return value;
        }
        private readonly Dictionary<string, FieldInfo> ___GetFieldCache = new Dictionary<string, FieldInfo>();

        private void SetBackingFieldValueExpression(object propValue, string propName, string ownerClassName, Type valueType)
        {
            if (!___SetBackingFieldValueExpressionCache.TryGetValue(propName, out var value))
            {
                var p = Expression.Parameter(valueType);
                value = Expression.Lambda(Expression.Assign(Expression.Field(Expression.Constant(this), GetField(ownerClassName, propName)), p), p).Compile();
                ___SetBackingFieldValueExpressionCache.Add(propName, value);
            }

            value.DynamicInvoke(propValue);
        }
        private readonly Dictionary<string, Delegate> ___SetBackingFieldValueExpressionCache = new Dictionary<string, Delegate>();

        private Dictionary<string, List<ProxyProperty>> Additionals = new Dictionary<string, List<ProxyProperty>>();

        public virtual void InitProxyProperties() { }

        public void AddProxyProperty(string property, ProxyProperty proxyProperty)
        {
            if (!Additionals.ContainsKey(property))
            {
                Additionals.Add(property, new List<ProxyProperty>());
            }

            Additionals[property].Add(proxyProperty);
        }

        #region Drawable

        public string Uid { get; } = Guid.NewGuid().ToString();

        public virtual string Icon { get; set; }

        public virtual string Name { get; set; }

        public IDrawColor BackgroundColor { get; set; }

        public IDrawColor ForegroundColor { get; set; }

        public virtual string Tileset { get; set; }

        public virtual Rectangle TileSetRegion { get; set; }

        public virtual Rectangle Region { get; set; }

        public virtual bool Container => false;

        /// <summary>
        /// Возвращает свойство типа T - реализация: case of types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T PropertyOfType<T>() where T : class => default;


        /// <summary>
        /// Возвращает свойства типов T - реализация: case of types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T[] PropertiesOfType<T>() where T : class => default;

        #endregion
    }

    public static class ProxyObjectExtensions
    {
        public static Func<TValue> ProxyBackingGet<TObject, TValue>(this ProxyObject proxyObject, Expression<Func<TObject, TValue>> property)
        {
            string prop = null;
            if (property.Body is MemberExpression memberExpression)
            {
                prop = memberExpression.Member.Name;
            }

            var get = proxyObject.BindGet(prop, typeof(TObject).AssemblyQualifiedName);

            return () => get().As<TValue>();
        }
    }
}