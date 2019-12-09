using System;

namespace Dungeon.Data.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class DataClassAttribute : Attribute
    {
        public Type DataType { get; set; }

        public DataClassAttribute(Type dataType)
        {
            if (!typeof(Persist).IsAssignableFrom(dataType))
            {
                throw new InvalidCastException($"Тип {dataType.Name} не может быть использовать как класс данных!");
            }
            DataType = dataType;
        }
    }
}
