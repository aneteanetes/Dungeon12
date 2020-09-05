using Dungeon.Engine.Editable.PropertyTable;
using Dungeon.Resources;
using Dungeon.Utils;
using Dungeon.Utils.ReflectionExtensions;
using Dungeon.View.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Dungeon.Engine.Projects
{
    public class DungeonEngineSceneObject : SimplePropertyTable
    {
        public string Name { get; set; }

        public string ClassName { get; set; }

        public bool Published { get; set; }

        [BsonIgnore]
        private Type _classType;

        [BsonIgnore]
        public Type ClassType
        {
            get
            {
                if (_classType == default)
                {
                    if (ClassName != default)
                    {
                        _classType = ResourceLoader.LoadType(ClassName);
                    }
                }

                return _classType;
            }
            set => _classType = value;
        }

        [BsonIgnore]
        public ISceneObject Instance { get; set; }

        public override void Commit()
        {
            if (Instance != default)
            {
                var ctors = Properties.Where(x => x.Name.Contains("Constructor")).ToList();
                var props = Properties.Except(ctors);
                foreach (var p in props)
                {
                    Instance.SetPropertyExprType(p.Name, p.Value, p.Type);
                }
            }

            base.Commit();
        }

        public ObservableCollection<DungeonEngineSceneObject> Nodes { get; set; } = new ObservableCollection<DungeonEngineSceneObject>();

        protected override List<PropertyTableRow> InitializePropertyTable()
        {
            var bodyProps = ClassType.GetProperties().Where(prop =>
            {
                var hidden = Attribute.GetCustomAttributes(prop)
                       .FirstOrDefault(x => x.GetType() == typeof(HiddenAttribute)) != default;
                if (hidden)
                    return false;

                if (!prop.CanWrite)
                    return false;

                return true;
            })
            .Select(x => new PropertyTableRow(x.Name, x.ValueAttribute<DefaultAttribute>() ?? x.PropertyType.GetDefault(), x.PropertyType))
            .ToList();

            var i = 0;
            foreach (var ctor in ClassType.GetConstructors().Where(x => x.GetParameters().Length > 0))
            {
                bodyProps.Add(new PropertyTableRow($"Constructor {i}", false, typeof(bool)));
                bodyProps.AddRange(ctor.GetParameters().Select(x => new PropertyTableRow(x.Name, x.ParameterType.GetDefault(), x.ParameterType)));
                i++;
            }

            return bodyProps;
        }
    }
}