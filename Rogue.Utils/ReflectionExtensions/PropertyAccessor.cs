namespace Rogue
{
    using FastMember;

    public static class PropertyAccessor
    {
        public static TValue GetProperty<TValue>(this object @object, string property)
        {
            var accessor = TypeAccessor.Create(@object.GetType(), true);
            return (TValue)accessor[@object, property];
        }

        public static TObject SetProperty<TObject, TValue>(this TObject @object, string property, TValue value)
        {
            var accessor = TypeAccessor.Create(@object.GetType(), true);
            accessor[@object, property] = value;
            return @object;
        }
    }
}