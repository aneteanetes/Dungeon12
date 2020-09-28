namespace Dungeon12.Drawing.SceneObjects.Map
{
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dungeon;

    public abstract class TooltipClickableSceneObject<T> : ClickActionSceneObject<T>
        where T : Dungeon.Physics.PhysicalObject
    {
        protected override ControlEventType[] Handles => new ControlEventType[]
        {
             ControlEventType.Focus,
             ControlEventType.Click,
             ControlEventType.Key
        };

        protected virtual string ClickableTooltipCursor { get; } = null;

        protected override Key[] KeyHandles => new Key[] { Key.LeftAlt, Key.RightAlt };

        public TooltipClickableSceneObject(PlayerSceneObject playerSceneObject, T @object, string tooltip) : base(playerSceneObject, @object, tooltip)
        {
        }

        protected abstract void OnTooltipClick();

        private void TooltipAction()
        {
            if (CheckActionAvailable(MouseButton.None))
            {
                OnTooltipClick();
            }
        }

        private ClickableTooltip clickableTooltip;

        private bool lootMode = false;

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            lootMode = true;
            if (!string.IsNullOrEmpty(TooltipText))
            {
                clickableTooltip = new ClickableTooltip(TooltipAction, TooltipText, new Point(this.ComputedPosition.X, this.ComputedPosition.Y - 0.8), this.TooltipTextColor)
                {
                    CacheAvailable = false,
                    AbsolutePosition = this.AbsolutePosition,
                    Layer = 1000,
                    Cursor = ClickableTooltipCursor
                };

                var boundInfo = new TooltipBoundInfo
                {
                    ClickableTooltip = clickableTooltip,
                    Parent = this
                };
                boundInfo.Refresh();

                AlignLocation(boundInfo);

                clickableTooltip.Destroy += () =>
                {
                    ClickableTooltipBoundsContainer.ClickableTooltipsBounds.Remove(boundInfo);
                    RefreshTooltips();
                };

                this.Destroy += () =>
                {
                    ClickableTooltipBoundsContainer.ClickableTooltipsBounds.Remove(boundInfo);
                    clickableTooltip?.Destroy?.Invoke();
                };
                this.ShowInScene(clickableTooltip.InList<ISceneObject>());
            }
        }

        private static void RefreshTooltips()
        {
            var boudTooltipInfoes = new List<TooltipBoundInfo>(ClickableTooltipBoundsContainer.ClickableTooltipsBounds);

            foreach (var boundTooltipInfo in ClickableTooltipBoundsContainer.ClickableTooltipsBounds)
            {
                boundTooltipInfo.Refresh();
            }

            ClickableTooltipBoundsContainer.ClickableTooltipsBounds.Clear();

            foreach (var boundTooltipInfo in boudTooltipInfoes)
            {
                AlignLocation(boundTooltipInfo);
            }
        }

        private static void AlignLocation(TooltipBoundInfo boundINfo, int top = 0)
        {
            var clickableTooltip = boundINfo.ClickableTooltip;

            var exists = ClickableTooltipBoundsContainer.ClickableTooltipsBounds.LastOrDefault(tooltip => tooltip.Bounds.IntersectsWith(boundINfo.Bounds));
            if (exists != null)
            {
                clickableTooltip.Top = exists.Bounds.Position.Y / 32;
                clickableTooltip.Top -= clickableTooltip.Height + 0.01;

                boundINfo.Bounds.Position.Y = clickableTooltip.Top * 32;

                AlignLocation(boundINfo, top + 1);
            }

            if (top == 0)
            {
                ClickableTooltipBoundsContainer.ClickableTooltipsBounds.Add(boundINfo);
            }
        }

        public override void KeyUp(Key key, KeyModifiers modifier)
        {
            lootMode = false;
            clickableTooltip?.Destroy?.Invoke();
            clickableTooltip = null;
        }

        public override void Focus()
        {
            base.Focus();
            if (lootMode)
            {
                aliveTooltip.Destroy?.Invoke();
                this.clickableTooltip.Opacity = 0.5;
            }
        }

        public override void Unfocus()
        {
            base.Unfocus();
            if (lootMode)
            {
                this.clickableTooltip.Opacity = 0.8;
            }
        }

        private class ClickableTooltip : Tooltip
        {
            private Action click;

            public ClickableTooltip(Action click, string text, Point position, IDrawColor drawColor) : base(text, position,drawColor)
            {
                this.click = click;
            }
            protected override ControlEventType[] Handles => new ControlEventType[] { ControlEventType.Click, ControlEventType.Focus };

            public override void Focus()
            {
                Opacity = 0.5;
                base.Focus();
            }

            public override void Unfocus()
            {
                Opacity = 0.8;
                base.Unfocus();
            }

            public override void Click(PointerArgs args)
            {
                Global.Interacting = true;
                click?.Invoke();
                base.Unfocus();
            }
        }
    }

    public class ClickableTooltipBoundsContainer
    {
        public static List<TooltipBoundInfo> ClickableTooltipsBounds = new List<TooltipBoundInfo>();
    }

    public class TooltipBoundInfo
    {
        public Tooltip ClickableTooltip { get; set; }

        public ISceneObject Parent { get; set; }

        public Dungeon.Physics.PhysicalObject Bounds { get; set; }

        public void Refresh()
        {
            ClickableTooltip.Left = Parent.ComputedPosition.X;
            ClickableTooltip.Top = Parent.ComputedPosition.Y - 0.8;

            Bounds = new Dungeon.Physics.PhysicalObject()
            {
                Position = new Dungeon.Physics.PhysicalPosition()
                {
                    X = ClickableTooltip.BoundPosition.X * 32,
                    Y = ClickableTooltip.BoundPosition.Y * 32
                },
                Size = new Dungeon.Physics.PhysicalSize()
                {
                    Width = ClickableTooltip.Width * 32,
                    Height = ClickableTooltip.Height * 32
                }
            };
        }
    }
}