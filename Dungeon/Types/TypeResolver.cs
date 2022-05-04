using System;
using System.Linq;
using System.Runtime.Loader;

namespace Dungeon.Types
{
    public class TypeResolver
    {
        public static Type GetTypeImpl(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                if (typeName.Contains("?"))
                {
                    var clearType = GetTypeImpl(typeName.Replace("?", ""));
                    if (clearType != null)
                        return typeof(Nullable<>).MakeGenericType(clearType);
                }

                foreach (var asm in AssemblyLoadContext.Default.Assemblies)
                {
                    type = asm?.GetTypes().FirstOrDefault(x => x.Name == typeName || x.FullName == typeName);
                    if (type != default)
                    {
                        break;
                    }
                }
            }

            return type;
        }
    }
}
