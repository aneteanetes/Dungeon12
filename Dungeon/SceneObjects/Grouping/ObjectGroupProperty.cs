using System;
using System.Collections.Generic;

namespace Dungeon.SceneObjects.Grouping
{
    public class ObjectGroupProperty
    {
        public bool Value { get; private set; }

        public Action<bool> OnSet = x => { };

        public void Set(bool value)
        {
            Value=value;
            OnSet(value);
            foreach (var item in Group)
            {
                if (item.Property==this)
                    continue;

                item.Property.Value=!value;
                item.Property.OnSet(!value);
            }
        }

        public void True()=>Set(true);

        public void False()=>Set(false);
        
        internal void BindGroup(List<ObjectGroupItem> group)
        {
            Group= group;
        }

        private List<ObjectGroupItem> Group { get; set; } = new();

        public static implicit operator bool(ObjectGroupProperty group) => group.Value;

        public static implicit operator ObjectGroupProperty(bool value) => throw new Exception("use Set instead!");
    }
}