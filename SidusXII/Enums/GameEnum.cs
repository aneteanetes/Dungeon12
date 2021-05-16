using Dungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SidusXII
{
    public class GameEnum
    {
        public static void Init()
        {
            var thisasm = MethodBase.GetCurrentMethod().DeclaringType.Assembly;

            var types = thisasm.GetTypes()
                .Where(x => x.BaseType.Name.ToString().Contains("GameEnum"));

            foreach (var type in types)
            {
                var props = type.GetProperties(BindingFlags.Static | BindingFlags.Public);
                for (int i = 0; i < props.Length; i++)
                {
                    var obj = type.NewAs<GameEnum>();
                    obj.Value = i;
                    obj.Display = props[i].Value<ValueAttribute, string>();
                    obj.PropertyName = props[i].Name;
                    obj.InitValue();

                    props[i].SetValue(null, obj);
                }
            }
        }

        protected virtual void InitValue()
        {

        }

        public static IEnumerable<T> AllValues<T>() => AllValues(typeof(T)).Select(x => x.As<T>());

        public static IEnumerable<GameEnum> AllValues(Type type)
            => type.GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Select(x => {
                    var obj = type.NewAs<GameEnum>();
                    obj.Display = x.Value<ValueAttribute, string>();
                    obj.Value = x.GetValue(null).As<GameEnum>().Value;
                    obj.PropertyName = x.Name;
                    obj.InitValue();

                    return obj;
                });

        public IEnumerable<GameEnum> AllValues() => AllValues(this.GetType());

        public int Value { get; set; }

        public string PropertyName { get; set; }

        public string Display { get; set; }

        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            if (Object.Equals(obj, null))
                return false;

            if (obj.GetType() != this.GetType())
                return false;

            var otherValue = obj.GetPropertyExprRaw(nameof(Value));

            return otherValue.Equals(Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(GameEnum x, GameEnum y)
        {
            if (Object.Equals(x,null))
            {
                return Object.Equals(y, null);
            }

            return x.Equals(y);
        }

        public static bool operator !=(GameEnum x, GameEnum y)
        {
            if (Object.Equals(x, null))
            {
                return !Object.Equals(y, null);
            }

            return !x.Equals(y);
        }
    }
}