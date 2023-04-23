using System.Collections.Generic;
using System.Linq;

namespace Dungeon.SceneObjects.Grouping
{
    public class ObjectGroup<TItem>
    {
        internal string PropertyName { get; set; }

        public List<ObjectGroupItem> List { get; set; } = new List<ObjectGroupItem>();

        public void Select(int idx=0)
        {
            List[idx].Property.True();
        }

        public void Init()
        {
            foreach (var item in List)
            {
                item.Property.BindGroup(List);
            }
        }

        public void Add(TItem item)
        {
            var groupItem = new ObjectGroupItem()
            {
                Property = item.GetPropertyExpr<ObjectGroupProperty>(PropertyName)
            };
            groupItem.Property.False();
            groupItem.Property.BindGroup(List);
            List.Add(groupItem);
        }
    }
}
