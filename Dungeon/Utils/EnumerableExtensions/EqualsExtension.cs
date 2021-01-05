using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dungeon.Utils.EnumerableExtensions
{
    public static class EqualsExtension
    {
        public static bool Equality(this object x, object y)
        {
            if (x == y)
                return true;

            string[] GetEqualityProps(object o)
            {
                return o.GetType()
                .GetProperties()
                .Where(x => 
                    x.CustomAttributes
                    .Any(a => a.AttributeType == typeof(EqualityAttribute)))
                .Select(x=>x.Name)
                .ToArray();
            }

            var xProps = GetEqualityProps(x);
            var yProps = GetEqualityProps(y);

            if (xProps.Length != yProps.Length)
                return false;

            if (!xProps.SequenceEqual(yProps))
                return false;

            foreach (var xProp in xProps)
            {
                if (!x.GetPropertyExprRaw(xProp).Equals(y.GetPropertyExprRaw(xProp)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
