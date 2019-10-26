using FastMember;
using Rogue.Types;
using Rogue.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Rogue.Proxy
{
    public class ProxyObject : IDrawable
    {
        private TypeAccessor _Type;

        public ProxyObject() => _Type = TypeAccessor.Create(this.GetType(),true);
        
        protected virtual string ProxyId => this.GetType().Name;

        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public Func<object> BindGet(string propName)
        {
            propName = $"___{propName}";
            if (!___BindGetCache.TryGetValue(propName, out var value))
            {
                value = () => _Type[this, propName];
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
        public Action<object> BindSet(string propName)
        {
            propName = $"___{propName}";
            if (!___BindSetCache.TryGetValue(propName, out var value))
            {
                value = v => _Type[this, propName] = v;
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

        private string GetProxyId(string prop) => $"{ProxyId}.{prop}";

        protected TCalculatedType Get<TCalculatedType>(TCalculatedType v, [CallerMemberName] string from = "")
        {
            var p = PropertyName(from);
            var get = BindGet(p);
            var set = BindSet(p);
            var proxy = Proxies(p);
            var proxyId = GetProxyId(p);

            return proxy.Get(v, proxyId, get, set);
        }

        protected void Set<TCalculatedType>(TCalculatedType v, [CallerMemberName] string from = "")
        {
            var p = PropertyName(from);
            var get = BindGet(p);
            var set = BindSet(p);
            var proxy = Proxies(p);
            var proxyId = GetProxyId(p);

            _Type[this, $"___{p}"] = proxy.Set(v, proxyId, get, set);
        }

        #region Drawable

        public string Uid { get; } = Guid.NewGuid().ToString();

        public string Icon { get; set; }

        public string Name { get; set; }

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
