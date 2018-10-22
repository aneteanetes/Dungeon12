namespace Rogue
{
    using System.Collections.Generic;
    using System.Linq;

    public static class IsEmptyExtension
    {
        public static bool IsNotEmpty<T>(this IEnumerable<T> @enum)
        {
            return @enum != null && @enum.Count() > 0;
        }
    }
}