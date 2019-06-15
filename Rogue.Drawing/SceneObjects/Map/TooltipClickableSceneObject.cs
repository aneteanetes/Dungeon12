namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.GUI;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class TooltipClickableSceneObject<T> : ClickActionSceneObject<T>
        where T : Physics.PhysicalObject
    {
        protected override ControlEventType[] Handles => new ControlEventType[]
        {
             ControlEventType.Focus,
             ControlEventType.Click,
             ControlEventType.Key
        };

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
                    Layer = 1000
                };

                var bounds = new Physics.PhysicalObject()
                {
                    Position = new Physics.PhysicalPosition() {
                        X = clickableTooltip.Position.X * 32,
                        Y = clickableTooltip.Position.Y * 32
                    },
                    Size = new Physics.PhysicalSize()
                    {
                        Width = clickableTooltip.Width * 32,
                        Height = clickableTooltip.Height * 32
                    }
                };
                AlignLocation(bounds, clickableTooltip);

                clickableTooltip.Destroy += () =>
                {
                    ClickableTooltipBoundsContainer.clickableTooltipsBounds.Remove(bounds);
                };

                this.Destroy += () =>
                {
                    ClickableTooltipBoundsContainer.clickableTooltipsBounds.Remove(bounds);
                    clickableTooltip?.Destroy?.Invoke();
                };
                this.ShowEffects(clickableTooltip.InList<ISceneObject>());
            }
        }

        private void AlignLocation(Physics.PhysicalObject bound, ClickableTooltip clickableTooltip, int top = 0)
        {
            var exists = ClickableTooltipBoundsContainer.clickableTooltipsBounds.LastOrDefault(tooltip => tooltip.IntersectsWith(bound));
            if (exists != null)
            {
                clickableTooltip.Top = exists.Position.Y / 32;
                clickableTooltip.Top -= clickableTooltip.Height + 0.01;

                bound.Position.Y = clickableTooltip.Top * 32;

                AlignLocation(bound, clickableTooltip, top+1);
            }

            if (top == 0)
            {
                ClickableTooltipBoundsContainer.clickableTooltipsBounds.Add(bound);
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

            public override void Focus() => Opacity = 0.5;

            public override void Unfocus() => Opacity = 0.8;

            public override void Click(PointerArgs args)
            {
                click?.Invoke();
            }
        }
    }

    public class ClickableTooltipBoundsContainer
    {
        public static List<Physics.PhysicalObject> clickableTooltipsBounds = new List<Physics.PhysicalObject>();
    }
}