using System;

namespace SidusXII
{
    public static class FluentBuilderExtensions
    {
        public static T Build<T>(this T obj, Action<T> buildAction)
        {
            buildAction?.Invoke(obj);
            return obj;
        }
    }
}
