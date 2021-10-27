namespace Dungeon12.Drawing.SceneObjects.UI
{
    using Dungeon.Control;
    using Dungeon12.Drawing.SceneObjects.Map;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class DropableControl : EmptyTooltipedSceneObject
    {
        public DropableControl() : base(null)
        {
        }
    }

    public abstract class DropableControl<TSource> : DropableControl
        where TSource : DraggableControl<TSource>
    {
        protected override ControlEventType[] Handles => new ControlEventType[]
        {
             ControlEventType.ClickRelease,
             ControlEventType.Focus
        };

        public DropableControl()
        {
            DragAndDropSceneControls.BindDropable(this);
        }

        public override void ClickRelease(PointerArgs args)
        {
            var draggable = DragAndDropSceneControls.GetDropped<TSource>(this);
            if (draggable != null && CheckDropAvailable(draggable as TSource))
            {
                draggable.GlobalClickRelease(args);
                draggable.DropProcessed++;
                //if (draggable.IntersectsWith(this)) // вот нахуя тут был Intersects ?
                {
                    OnDrop(draggable as TSource);
                }
            }
        }

        protected bool DropAvailable;

        public override void Focus()
        {
            var draggable = DragAndDropSceneControls.GetDropped<TSource>(this);
            if (draggable != null && CheckDropAvailable(draggable as TSource))
            {
                DropAvailable = true;
            }

            base.Focus();
        }

        protected virtual bool CheckDropAvailable(TSource source) => true;

        protected virtual void OnDrop(TSource source) { }

        public Action<TSource> OnDropAction { get; set; }
    }

    public class DragAndDropSceneControls
    {
        private static int _draggableLayers;
        public static int DraggableLayers
        {
            get => _draggableLayers;
            set
            {
                _draggableLayers = value;
                if (_draggableLayers < 0)
                    _draggableLayers = 0;
            }
        }

        private static Dictionary<DropableControl, List<DraggableControl>> subscribers = new Dictionary<DropableControl, List<DraggableControl>>();
        private static List<DraggableControl> free = new List<DraggableControl>();

        public static void BindDropable(DropableControl dropable)
        {
            dropable.Destroy += () =>
            {
                if (subscribers.TryGetValue(dropable, out var subs))
                {
                    foreach (var draggable in subscribers[dropable])
                    {
                        if (!free.Contains(draggable))
                        {
                            free.Add(draggable);
                        }
                    }
                    subscribers.Remove(dropable);
                }
            };

            var publishers = new List<DraggableControl>();
            publishers.AddRange(free.Where(x => x.GetType() == dropable.GetType().BaseType.GetGenericArguments()[0]));
            publishers.AddRange(subscribers.Where(x => x.Key.GetType().BaseType.GetGenericArguments()[0] == dropable.GetType().BaseType.GetGenericArguments()[0]).SelectMany(x => x.Value));

            subscribers.Add(dropable, publishers);
        }

        public static void BindDragable(DraggableControl draggable)
        {
            var dropables = subscribers.Where(x => x.Key.GetType().BaseType.GetGenericArguments()[0] == draggable.GetType());

            foreach (var dropable in dropables)
            {
                if (dropable.Value != null)
                {
                    draggable.Destroy += () =>
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

        public static DraggableControl<T> GetDropped<T>(DropableControl dropable)
        {
            return subscribers[dropable]
                .Cast<DraggableControl<T>>()
                .FirstOrDefault(x => x == LastDragged);
        }

        private static DraggableControl LastDragged;

        public static bool IsDragging => LastDragged != null;

        public static void SetDragged(DraggableControl draggable)
        {
            LastDragged = draggable;
        }
    }
}