using Dungeon.Transactions;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using FastMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Dungeon.Proxy
{
    public class ProxyObject : Applicable, IDrawable
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
            var key = new CompositeTypeKey<string>()
            {
                Owner = this.GetType(),
                Value = propName
            };
            if (!___BindGetCache.TryGetValue(key, out var value))
            {
                value = () => GetBackginFieldValueExpression(ownerClassName, propName);// GetBackingFieldValue(propName, ownerClassName);
                ___BindGetCache.Add(key, value);
            }

            return value;
        }
        private static readonly Dictionary<CompositeTypeKey<string>, Func<object>> ___BindGetCache = new Dictionary<CompositeTypeKey<string>, Func<object>>();

        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public Action<object> BindSet<TValue>(string propName, string ownerclassname)
        {
            propName = $"___{propName}";
            var key = new CompositeTypeKey<string>()
            {
                Owner = this.GetType(),
                Value = propName
            };
            if (!___BindSetCache.TryGetValue(key, out var value))
            {
                value = v => SetBackingFieldValueExpression(v, propName, ownerclassname, typeof(TValue));
                ___BindSetCache.Add(key, value);
            }

            return value;
        }
        private static readonly Dictionary<CompositeTypeKey<string>, Action<object>> ___BindSetCache = new Dictionary<CompositeTypeKey<string>, Action<object>>();

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
            var key = new CompositeTypeKey<string>()
            {
                Owner = this.GetType(),
                Value = prop
            };
            if (!___ProxiedCache.TryGetValue(key, out var value))
            {
                value = (ProxiedAttribute)_Type.GetMembers().FirstOrDefault(m => m.Name == prop)?.GetAttribute(typeof(ProxiedAttribute), false);
                ___ProxiedCache.Add(key, value);
            }

            return value;
        }
        private static readonly Dictionary<CompositeTypeKey<string>, ProxiedAttribute> ___ProxiedCache = new Dictionary<CompositeTypeKey<string>, ProxiedAttribute>();

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

        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public object GetBackginFieldValueExpression(string ownerClassName, string propName)
        {
            var key = new CompositeTypeKey<string>()
            {
                Owner = this.GetType(),
                Value = propName
            };

            if (!___GetBackginFieldValueExpressionCache.TryGetValue(key, out var value))
            {
                var p = Expression.Parameter(this.GetType());

                var ownerType = Type.GetType(ownerClassName);
                var baseType = this.GetType().BaseType;
                if (ownerType.IsGenericType && baseType.IsGenericType && baseType.GetGenericTypeDefinition() == ownerType)
                {
                    p = Expression.Parameter(p.Type.BaseType);
                    value = Expression.Lambda(Expression.Field(p, propName), p).Compile();
                }
                else
                {
                    value = Expression.Lambda(Expression.Field(p, GetField(ownerClassName, propName)), p).Compile();
                }

                ___GetBackginFieldValueExpressionCache.Add(key, value);
            }

            return value.DynamicInvoke(this);
        }

        private static readonly Dictionary<CompositeTypeKey<string>, Delegate> ___GetBackginFieldValueExpressionCache = new Dictionary<CompositeTypeKey<string>, Delegate>();


        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public FieldInfo GetField(string ownerClassName, string propName)
        {
            var key = new CompositeTypeKey<string>()
            {
                Owner = this.GetType(),
                Value = ownerClassName + propName
            };
            if (!___GetFieldCache.TryGetValue(key, out var value))
            {
                value = Type.GetType(ownerClassName).GetField(propName, BindingFlags.Instance | BindingFlags.NonPublic);
                ___GetFieldCache.Add(key, value);
            }

            return value;
        }
        private static readonly Dictionary<CompositeTypeKey<string>, FieldInfo> ___GetFieldCache = new Dictionary<CompositeTypeKey<string>, FieldInfo>();

        private void SetBackingFieldValueExpression(object propValue, string propName, string ownerClassName, Type valueType)
        {
            var key = new CompositeTypeKey<string>()
            {
                Owner = this.GetType(),
                Value = propName
            };

            if (!___SetBackingFieldValueExpressionCache.TryGetValue(key, out var value))
            {
                var pType = Expression.Parameter(this.GetType());
                var p = Expression.Parameter(valueType);

                var ownerType = Type.GetType(ownerClassName);
                var baseType = this.GetType().BaseType;
                if (ownerType.IsGenericType && baseType.IsGenericType && baseType.GetGenericTypeDefinition() == ownerType)
                {
                    pType = Expression.Parameter(pType.Type.BaseType);
                    value = Expression.Lambda(Expression.Assign(Expression.Field(pType, propName), p), pType, p).Compile();

                }
                else
                {
                    value = Expression.Lambda(Expression.Assign(Expression.Field(pType, GetField(ownerClassName, propName)), p), pType, p).Compile();
                }

                ___SetBackingFieldValueExpressionCache.Add(key, value);
            }

            value.DynamicInvoke(this,propValue);
        }
        private static readonly Dictionary<CompositeTypeKey<string>, Delegate> ___SetBackingFieldValueExpressionCache = new Dictionary<CompositeTypeKey<string>, Delegate>();

        private Dictionary<string, List<ProxyProperty>> Additionals = new Dictionary<string, List<ProxyProperty>>();

        public virtual void InitProxyProperties() { }

        public virtual void FreeProxyProperties()
        {
            this.Additionals.Clear();
        }

        public void AddProxyProperty(string property, ProxyProperty proxyProperty)
        {
            if (!Additionals.ContainsKey(property))
            {
                Additionals.Add(property, new List<ProxyProperty>());
            }

            Additionals[property].Add(proxyProperty);
        }

        public void RemoveProxyProperty(string property, ProxyProperty proxyProperty)
        {
            if (Additionals.ContainsKey(property))
            {
                Additionals[property].Remove(proxyProperty);
            }
        }

        #region Drawable

        public string Uid { get; set; } = Guid.NewGuid().ToString();

        public virtual string Icon { get; set; }

        private string _image;
        public virtual string Image { get => _image ?? Icon; set => _image = value; }

        public virtual string Name { get; set; }

        public IDrawColor BackgroundColor { get; set; }

        public IDrawColor ForegroundColor { get; set; }

        public virtual string Tileset { get; set; }

        public virtual Square TileSetRegion { get; set; }

        public virtual Square TileSetRegionStart { get; set; }

        public virtual Square Region { get; set; }

        public virtual bool Container => false;

        public ISceneObject SceneObject { get; set; }

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

        public virtual void SetView(ISceneObject sceneObject)
        {
            this.SceneObject = sceneObject;
}

        protected override void CallApply(dynamic obj) { }

        protected override void CallDiscard(dynamic obj) { }

        public virtual void Destroy(){ this.SceneObject?.Destroy(); }

        public void Init() { }

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