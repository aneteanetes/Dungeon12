using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dungeon.SceneObjects.Grouping
{
    public class ObjectGroupBuilder<TItem>
    {
        private List<TItem> _items = new();

        public ObjectGroupBuilder(params TItem[] items)
        {
            _items.AddRange(items);
        }

        public TItem Add(TItem item)
        {
            _items.Add(item);
            return item;
        }

        private string PropertyName;

        public ObjectGroupBuilder<TItem> Property(Expression<Func<TItem, ObjectGroupProperty>> selector)
        {
            PropertyName=(selector.Body as MemberExpression).Member.Name;
            return this;
        }

        public ObjectGroup<TItem> Build()
        {
            var group = new ObjectGroup<TItem>
            {
                List = _items.Select(x => new ObjectGroupItem
                {
                    Property = x.GetPropertyExpr<ObjectGroupProperty>(PropertyName)
                }).ToList()
            };

            group.Init();

            return group;
        }
    }
}
