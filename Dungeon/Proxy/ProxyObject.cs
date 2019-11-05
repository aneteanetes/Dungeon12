using FastMember;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

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
        public Func<object> BindGet(string propName,string ownerClassName)
        {
            propName = $"___{propName}";
            if (!___BindGetCache.TryGetValue(propName, out var value))
            {
                value = () => GetBackingFieldValue(propName, ownerClassName);
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
        public Action<object> BindSet(string propName,string ownerclassname)
        {
            propName = $"___{propName}";
            if (!___BindSetCache.TryGetValue(propName, out var value))
            {
                value = v => SetBackingFieldValue(v, propName, ownerclassname);
                ___BindSetCache.Add(propName, value);
            }

            return value;
        }
        private readonly Dictionary<string, Action<object>> ___BindSetCache = new Dictionary<string, Action<object>>();

        private string PropertyName(string callerProp)
        {
            return callerProp;
        }

        private ProxiedAttribute Proxies(string prop) => (ProxiedAttribute)_Type.GetMembers().FirstOrDefault(m => m.Name == prop)?.GetAttribute(typeof(ProxiedAttribute), false);

        private ProxyProperty[] ProxiesAdditional(string prop)
        {
            if (!Additionals.TryGetValue(prop, out var vals))
            {
                return new ProxyProperty[0];
            }
            return vals.ToArray();
        }

        private string GetProxyId(string prop) => $"{ProxyId}.{prop}";

        protected TCalculatedType Get<TCalculatedType>(TCalculatedType v, string ownerClassName, [CallerMemberName] string from = "")
        {
            var p = PropertyName(from);
            var get = BindGet(p, ownerClassName);
            var set = BindSet(p, ownerClassName);
            var proxy = Proxies(p);
            var add = ProxiesAdditional(p);
            var proxyId = GetProxyId(p);

            return proxy.Get(v, proxyId, get, set, this, _Type,p, add);
        }
        
        protected void Set<TCalculatedType>(TCalculatedType v, string ownerClassName, [CallerMemberName] string from = "")
        {
            var p = PropertyName(from);
            var get = BindGet(p,ownerClassName);
            var set = BindSet(p,ownerClassName);
            var proxy = Proxies(p);
            var add = ProxiesAdditional(p);
            var proxyId = GetProxyId(p);

            var backingField = $"___{p}";

            var settedValue = proxy.Set(v, proxyId, get, set, this, _Type, p,add);

            SetBackingFieldValue(settedValue, backingField, ownerClassName);
        }

        private Dictionary<string, FieldInfo> FastMemberCantAccess = new Dictionary<string, FieldInfo>();

        private object GetBackingFieldValue(string propName, string ownerClassName)
        {
            if (!FastMemberCantAccess.ContainsKey(propName))
            {
                try
                {
                    return _Type[this, propName];
                }
                catch (ArgumentException)
                {
                    var field = Type.GetType(ownerClassName).GetField(propName, BindingFlags.Instance | BindingFlags.NonPublic);
                    FastMemberCantAccess.Add(propName, field);
                }
            }
            return FastMemberCantAccess[propName].GetValue(this);
        }

        private void SetBackingFieldValue(object value, string propName, string ownerClassName)
        {
            if (!FastMemberCantAccess.ContainsKey(propName))
            {
                try
                {
                    _Type[this, propName] = value;
                    return;
                }
                catch (ArgumentException)
                {
                    var field = Type.GetType(ownerClassName).GetField(propName, BindingFlags.Instance | BindingFlags.NonPublic);
                    FastMemberCantAccess.Add(propName, field);
                }
            }
            FastMemberCantAccess[propName].SetValue(this, value);
        }

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
}
