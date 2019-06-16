namespace Rogue.Drawing.SceneObjects.UI
{
    using Rogue.Control.Events;
    using Rogue.Control.Pointer;
    using System.Collections.Generic;
    using System.Linq;

    public class DropableControl : HandleSceneControl
    {

    }

    public class DropableControl<TSource> : DropableControl
        where TSource : DraggableControl
    {
        protected override ControlEventType[] Handles => new ControlEventType[]
        {
             ControlEventType.ClickRelease
        };

        public DropableControl()
        {
            DragAndDropSceneControls.BindDropable(this);
        }

        public override void ClickRelease(PointerArgs args)
        {
            var draggable = DragAndDropSceneControls.GetDropped(this);
            if (draggable != null)
            {
                if (draggable.IntersectsWith(this))
                {
                    OnDrop(draggable as TSource);
                }
            }
        }


        protected virtual void OnDrop(TSource source) { }
    }

    public class DragAndDropSceneControls
    {
        private static Dictionary<DropableControl, List<DraggableControl>> subscribers = new Dictionary<DropableControl, List<DraggableControl>>();
        private static List<DraggableControl> free = new List<DraggableControl>();

        public static void BindDropable(DropableControl dropable)
        {
            dropable.Destroy += () =>
            {
                foreach (var draggable in subscribers[dropable])
                {
                    if(!free.Contains(draggable))
                    {
                        free.Add(draggable);
                    }
                }
                subscribers.Remove(dropable);
            };

            var publishers = new List<DraggableControl>();
            publishers.AddRange(free.Where(x => x.GetType() == dropable.GetType().BaseType.GetGenericArguments()[0]));
            publishers.AddRange(subscribers.Where(x => x.Key.GetType().BaseType.GetGenericArguments()[0] == dropable.GetType().BaseType.GetGenericArguments()[0]).SelectMany(x=>x.Value));

            subscribers.Add(dropable, publishers);
        }

        public static void BindDragable(DraggableControl draggable)
        {
            var dropables = subscribers.Where(x => x.Key.GetType().BaseType.GetGenericArguments()[0] == draggable.GetType());

            foreach (var dropable in dropables)
            {
                if (dropable.Value != null)
                {
                    draggable.Destroy = () =>
                    {
                        free.Remove(draggable);
                        var subs = subscribers.Where(x => x.Key.GetType().BaseType.GetGenericArguments()[0] == draggable.GetType());
                        foreach (var sub in subs)
                        {
                            if (sub.Value != null)
                            {
                                sub.Value.Remove(draggable);
                            }
                        }
                    };
                    dropable.Value.Add(draggable);
                }
            }
        }

        public static DraggableControl GetDropped(DropableControl dropable)
        {
            return subscribers[dropable].FirstOrDefault(x => x == LastDragged);
        }

        private static DraggableControl LastDragged;

        public static void SetDragged(DraggableControl draggable)
        {
            LastDragged = draggable;
        }
    }
}