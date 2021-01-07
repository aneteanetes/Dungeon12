using Dungeon.Engine.Editable.ObjectTreeList;
using Dungeon.Engine.Editable.PropertyTable;
using Dungeon.Engine.Events;
using Dungeon.Engine.Projects;
using Dungeon.Resources;
using Dungeon.Utils;
using Dungeon.View.Interfaces;
using LiteDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Dungeon.Engine.Editable.Structures
{
    public class StructureSceneObject : StructureObject
    {
        [BsonIgnore]
        public override StructureObjectType StructureType => StructureObjectType.Object;

        public StructureSceneObject()
        {
            this.BindEmbeddedIcon("DeploymentFileStatusBar5_16x");
            this.CollectionValueChanged += SceneObjectTypeChanged;
        }

        [Hidden]
        public SceneObject SceneObject { get; set; }

        [Hidden]
        [BsonIgnore]
        public SceneObjectClass SceneObjectTypeSelected => this.Get(nameof(SceneObjectType))?.Value?.As<SceneObjectClass>();

        [Title("Тип объекта")]
        [BsonIgnore]
        public ObservableCollection<SceneObjectClass> SceneObjectType { get; set; }

        private void SceneObjectTypeChanged(IEnumerable @enum, object obj)
        {
            var objType = obj.As<SceneObjectClass>();
            SceneObject = new SceneObject()
            {
                ClassName = objType.ClassName,
                Name = this.Name,
            };
        }

        protected override List<PropertyTableRow> InitializePropertyTable()
        {
            InitializeObjectClasses();
            return base.InitializePropertyTable();
        }

        private void InitializeObjectClasses()
        {
            this.SceneObjectType = new ObservableCollection<SceneObjectClass>(
                ResourceLoader.LoadTypes<ISceneObject>()
                .Where(x => x.IsClass && !x.IsAbstract && Attribute.GetCustomAttribute(x, typeof(HiddenAttribute)) == default)
                .Select(x => new SceneObjectClass()
                {
                    Name = (Attribute.GetCustomAttribute(x, typeof(DisplayNameAttribute)) as DisplayNameAttribute)?.DisplayName ?? x.Name,
                    ClassName = x.FullName,
                    ClassType = x
                }).OrderBy(x => x.Name));
        }

        [Title("Обновить список типов")]
        [Visible]
        public void RefreshTypes()
        {
            InitializeObjectClasses();
        }

        [Title("Опубликовать")]
        [Visible]
        public void Publish()
        {
            if (this.SceneObject != default)
                DungeonGlobal.Events.Raise(new PublishSceneObjectEvent(this.SceneObject));
        }

        public override void InitRuntime()
        {
            InitializeObjectClasses();
        }

        public override ObjectTreeListItem Clone()
        {
            var newCopy = new StructureSceneObject();

            FillClone(newCopy);

            newCopy.SceneObject = SceneObject.Clone();

            return newCopy;
        }
    }
}
