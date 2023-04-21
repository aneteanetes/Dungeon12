using System.Collections.Generic;

namespace Dungeon.SceneObjects.Grouping
{
    public class ObjectGroup<TItem>
    {
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
    }
}
