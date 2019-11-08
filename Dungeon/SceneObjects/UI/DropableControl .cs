namespace Dungeon.Drawing.SceneObjects.UI
{
    using Dungeon.Control;
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.GameObjects;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IDropableControl : ISceneObjectControl
    {
    }

    public abstract class DropableControl<TSource> : TooltipedSceneObject<EmptyGameComponent>, IDropableControl
        where TSource : DraggableControl<TSource>
    {
        protected override ControlEventType[] Handles => new ControlEventType[]
        {
             ControlEventType.ClickRelease,
             ControlEventType.Focus
        };

        public DropableControl():base(EmptyGameComponent.Empty,"")
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
        public static int DraggableLayers = 1;

        private static Dictionary<IDropableControl, List<IDraggableControl>> subscribers = new Dictionary<IDropableControl, List<IDraggableControl>>();
        private static List<IDraggableControl> free = new List<IDraggableControl>();

        public static void BindDropable(IDropableControl dropable)
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

            var publishers = new List<IDraggableControl>();
            publishers.AddRange(free.Where(x => x.GetType() == dropable.GetType().BaseType.GetGenericArguments()[0]));
            publishers.AddRange(subscribers.Where(x => x.Key.GetType().BaseType.GetGenericArguments()[0] == dropable.GetType().BaseType.GetGenericArguments()[0]).SelectMany(x=>x.Value));

            subscribers.Add(dropable, publishers);
        }

        public static void BindDragable(IDraggableControl draggable)
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

        public static DraggableControl<T> GetDropped<T>(IDropableControl dropable)
            where T : IGameComponent
        {
            return subscribers[dropable].FirstOrDefault(x => x == LastDragged).As<DraggableControl<T>>();
        }

        private static IDraggableControl LastDragged;

        public static bool IsDragging => LastDragged != null;

        public static void SetDragged(IDraggableControl draggable)
        {
            LastDragged = draggable;
        }
    }
}