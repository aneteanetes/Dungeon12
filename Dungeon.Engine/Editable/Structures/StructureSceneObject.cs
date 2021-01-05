using Dungeon.Engine.Editable.ObjectTreeList;
using Dungeon.Engine.Editable.PropertyTable;
using Dungeon.Engine.Projects;
using Dungeon.Resources;
using Dungeon.Utils;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Dungeon.Engine.Editable.Structures
{
    public class StructureSceneObject : StructureObject
    {
        public override StructureObjectType StructureType => StructureObjectType.Object;

        public StructureSceneObject()
        {
            this.BindEmbeddedIcon("DeploymentFileStatusBar5_16x");
        }

        [Hidden]
        public SceneObject SceneObject { get; set; }

        [Title("Тип объекта")]
        public ObservableCollection<SceneObjectClass> SceneObjectType { get; set; }

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
