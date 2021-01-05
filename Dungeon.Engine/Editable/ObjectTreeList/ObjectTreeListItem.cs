using Dungeon.Engine.Editable.PropertyTable;
using Dungeon.Utils;
using Dungeon.Utils.ReflectionExtensions;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Dungeon.Engine.Editable.ObjectTreeList
{
    public class ObjectTreeListItem : SimplePropertyTable
    {
        [BsonIgnore]
        [Hidden]
        public ObjectTreeListItem Parent { get; set; }

        [Hidden]
        public ObservableCollection<ObjectTreeListItem> Nodes { get; set; } = new ObservableCollection<ObjectTreeListItem>();

        public string Name { get; set; }

        private string _image;

        [Hidden]
        public string Image
        {
            get => _image == default
                    ? template.Replace("@", "Cube_16x")
                    : _image;

            private set => _image = value;
        }

        private static string template = $"pack://siteoforigin:,,,/Icons/@.png";

        public virtual void Remove()
        {
            this.Parent.Nodes.Remove(this);
        }

        public virtual void CopyInParent()
        {
            this.Parent?.Nodes.Add(this.Clone());
        }

        public virtual ObjectTreeListItem Clone()
        {
            var obj = new ObjectTreeListItem();
            FillClone(obj);
            return obj;
        }

        protected virtual void FillClone(ObjectTreeListItem clone)
        {
            clone.Name = Name;
            clone.Parent = Parent;
            clone.Nodes = CloneNodes();
            clone._image = this._image;
        }

        public virtual ObservableCollection<ObjectTreeListItem> CloneNodes()
        {
            return new ObservableCollection<ObjectTreeListItem>(Nodes.Select(x => x.Clone()));
        }

        public void BindEmbeddedIcon(string name)
        {
            _image = template.Replace("@", name.Replace(".png", ""));
        }

        public override void Commit()
        {
            if (!this.IsInitialized)
                return;

            this.PropertyTable.ForEach(row =>
            {
                if (!row.Type.IsEnumerable())
                {
                    this.SetPropertyExprType(row.Name, row.Value, row.Type);
                }
            });
        }
    }
}