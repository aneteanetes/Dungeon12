using FastMember;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon
{
    public static class RecursiveFill
    {
        public static object CreateAndFill(this string rootType, IDictionary<object, object> data)
        {
            var obj = Type.GetType(rootType).New();

            SetProperties(obj, data);

            return obj;
        }

        private static void SetProperties(object obj, IDictionary<object,object> props)
        {
            if (obj == null)
            {
                obj = obj.GetType().New();
            }

            var accessor = TypeAccessor.Create(obj.GetType());

            foreach (var prop in props)
            {
                obj.SetProperty(prop.Key.ToString(), prop.Value);

                if(prop.Value is IDictionary<object,object> innerProp)
                {
                    SetProperties(obj.GetProperty(prop.Key.ToString()), innerProp);
                }
            }
        }
    }
}
