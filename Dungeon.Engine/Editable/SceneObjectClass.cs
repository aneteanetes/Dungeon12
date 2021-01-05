using Dungeon.Resources;
using Dungeon.Utils;
using LiteDB;
using System;
using System.Collections.ObjectModel;

namespace Dungeon.Engine.Projects
{
    public class SceneObjectClass
    {
        public string Name { get; set; }

        [Equality]
        public string ClassName { get; set; }

        private Type _type;

        [BsonIgnore]
        public Type ClassType
        {
            get
            {
                if (_type == default && ClassName != default)
                {
                    try
                    {
                        _type = ResourceLoader.LoadType(ClassName);
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
                ClassName = _type.FullName;
            }
        }
    }
}