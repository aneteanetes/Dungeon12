using Dungeon.Resources;
using LiteDB;
using System;

namespace Dungeon.Engine.Editable.PropertyTable
{
    public class PropertyTableRow
    {
        public PropertyTableRow() { }

        public PropertyTableRow(string name, object value, Type type)
        {
            this.Name = name;
            this.Value = value;
            this.Type = type;
        }

        public string Name { get; set; }

        public object Value { get; set; }        

        public string TypeName { get; set; }

        private Type _type;

        [BsonIgnore]
        public Type Type
        {
            get
            {
                if (_type == default && TypeName != default)
                {
                    try
                    {
                        _type = ResourceLoader.LoadType(TypeName);
                    }
                    catch (Exception)
                    {

                    }
                }

                return _type;
            }
            set
            {
                _type = value;
                TypeName = _type.FullName;
            }
        }
    }
}
