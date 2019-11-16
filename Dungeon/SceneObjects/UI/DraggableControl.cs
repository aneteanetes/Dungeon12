namespace Dungeon.Drawing.SceneObjects.UI
{
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class DraggableControl : EmptyTooltipedSceneObject
    {
        public DraggableControl() : base(null)
        {
        }
    }

    public abstract class DraggableControl<T> : DraggableControl
    {
        public override bool DrawOutOfSight => true;

        public virtual bool DestroyOnEscape => true;

        public int DropProcessed { get; set; }

        public bool DisableDrag { get; set; }

        protected override Key[] KeyHandles => new Key[] { Key.Escape }.Concat(OverrideKeyHandles).ToArray();

        protected virtual Key[] OverrideKeyHandles => new Key[0];

        public virtual bool TextureDragging { get; set; } = false;

        public override int Layer { get; set; } = 50;

        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        /// <summary>
        /// Блокировать контроль сцены пока объект существует
        /// </summary>
        public virtual bool BlockSceneControls { get; set; } = true;

        public DraggableControl()
        {
            this.ZIndex = ++DragAndDropSceneControls.DraggableLayers;
            this.Destroy += () =>
            {
                if (BlockSceneControls)
                {
                    Global.BlockSceneControls = false;
                }
                DragAndDropSceneControls.DraggableLayers--;
            };

            if (BlockSceneControls)
            {
                Global.BlockSceneControls = true;
            }
            DragAndDropSceneControls.BindDragable(this);
        }

        private ControlEventType[] handles()
        {
            IEnumerable<ControlEventType> origin = new ControlEventType[]
            {
                ControlEventType.Click,
                ControlEventType.GlobalClickRelease,
                ControlEventType.Key,
                ControlEventType.Focus
            };

            if (!TextureDragging)
            {
                origin = origin.Concat(ControlEventType.MouseMove.InEnumerable());
            }

            origin = origin.Concat(OverrideHandles);

            return origin.ToArray();
        }

        protected override ControlEventType[] Handles => handles();

        protected virtual ControlEventType[] OverrideHandles => new ControlEventType[0];

        private bool drag = false;

        private PointerArgs delta = null;

        public override void MouseMove(PointerArgs args)
        {
            if (drag)
            {
                var argsX = args.X / 32;
                var deltaX = delta.X / 32;

                var argsY = args.Y / 32;
                var deltaY = delta.Y / 32;

                if (argsX > deltaX)
                {
                    this.Left -= deltaX - argsX;
                }
                else if (argsX != deltaX)
                {
                    this.Left += argsX - deltaX;
                }

                if (argsY > deltaY)
                {
                    this.Top -= deltaY - argsY;
                }
                else if (argsY != deltaY)
                {
                    this.Top += argsY - deltaY;
                }
            }

            delta = args;

            base.MouseMove(args);
        }

        private Point dragOffset = new Point(0, 0);

        public override void Click(PointerArgs args)
        {
            if (DisableDrag)
                return;

            if (args.MouseButton == MouseButton.Left)
            {
                if (TextureDragging)
                {
                    Global.DrawClient.Drag(this);
                    this.Visible = false;
                }

                dragOffset = new Point()
                {
                    X = this.Left - args.X / 32,
                    Y = this.Top - args.Y / 32,
                };

                DropProcessed = 0;

                delta = args;
                dropped = false;
                drag = true;
                OnDrag?.Invoke();
                DragAndDropSceneControls.SetDragged(this);
                UpLayer();
                base.Click(args);
            }
        }

        public Action OnDrag { get; set; }

        public Action OnDrop { get; set; }

        private bool dropped = true;

        public override void GlobalClickRelease(PointerArgs args)
        {
            if (DisableDrag)
                return;

            if (dropped)
                return;

            if (drag)
            {
                dropped = true;
                if (TextureDragging)
                {
                    Global.DrawClient.Drop();
                    this.Visible = true;
                }

                this.Left = args.X / 32 + this.dragOffset.X;
                this.Top = args.Y / 32 + this.dragOffset.Y;

                drag = false;

                DragAndDropSceneControls.SetDragged(null);
                DownLayer();
            }
            base.GlobalClickRelease(args);
        }

        private void UpLayer()
        {
            this.Layer = 1000;
            SetChainLayer(this, 1000);
        }

        private Action OnDownLayer;
        private void DownLayer()
        {
            this.Layer = 50;
            OnDownLayer?.Invoke();
            OnDownLayer = null;
        }

        private void SetChainLayer(ISceneObject @object, int value)
        {
            if (@object.Parent != null)
            {
                var currentLayer = @object.Parent.Layer;
                OnDownLayer += () => @object.Parent.Layer = currentLayer;
                @object.Parent.Layer = value;
                SetChainLayer(@object.Parent, value);
            }
        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (this.DestroyOnEscape && key == Key.Escape)
            {
                this.Destroy?.Invoke();
            }
        }

        protected override void AddChild(ISceneObjectControl sceneObject)
        {
            sceneObject.ZIndex = this.ZIndex;
            base.AddChild(sceneObject);
        }
    }
}