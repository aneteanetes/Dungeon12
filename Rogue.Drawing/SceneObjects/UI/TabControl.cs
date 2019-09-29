namespace Rogue.Drawing.SceneObjects.UI
{
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.Impl;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class TabControl<TContent, TArgument, TTab> : HandleSceneControl
        where TContent : ISceneObject
        where TTab : TabControl<TContent, TArgument, TTab>
    {
        /// <summary>
        /// здесь юзается хак static generics
        /// </summary>
        private static TContent currentTabContent = default;

        private static Action<TabControl<TContent, TArgument,TTab>> InactiveOther;

        public static TTab Current { get; private set; }

        public static Action<TTab> OnChange { get; set; }

        protected abstract TTab Self { get; }

        internal bool active = false;

        private readonly bool disabled;

        private readonly TArgument argument;

        internal readonly SceneObject parent;

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.Focus,
            ControlEventType.Click
        };

        public TabControl(SceneObject parent, bool active, TArgument argument = default, string title = null, string titleImg = null)
        {
            InactiveOther += SetInactive;
            Destroy += () => { InactiveOther -= this.SetInactive; };

            this.Height = 2;
            this.Width = 3;

            if (title != null)
            {
                this.AddTextCenter(new DrawText(title));
            }

            if (titleImg != null)
            {
                this.AddControlCenter(new ImageControl(titleImg)
                {
                    AbsolutePosition = true,
                    CacheAvailable = false,
                    Width = 1,
                    Height = 1
                });
            }

            this.disabled = argument == default
                || title == null && titleImg == null;

            this.parent = parent;
            this.argument = argument;
            this.active = active;

            if (active == true)
            {
                Current = Self;
                OnChange?.Invoke(Self);
            }

            this.Image = SquareTexture(false);
        }

        protected virtual void SetInactive(TabControl<TContent, TArgument, TTab> tab)
        {
            if (tab != this)
            {
                this.active = false;
                this.Image = SquareTexture(false);
            }
        }

        private string SquareTexture(bool focus)
        {
            if (disabled)
                return $"Rogue.Resources.Images.ui.squareWeapon_h_d.png";

            var f = focus || active
                ? "_f"
                : "";

            return $"Rogue.Resources.Images.ui.squareWeapon_h{f}.png";
        }

        public override void Focus()
        {
            this.Image = SquareTexture(true);
            base.Focus();
        }

        public override void Unfocus()
        {
            this.Image = SquareTexture(false);
            base.Unfocus();
        }

        public override void Click(PointerArgs args) => DoOpen();

        public void Open() => DoOpen();

        private void DoOpen()
        {
            if (!disabled)
            {
                active = true;
                InactiveOther(this);

                if (currentTabContent != null)
                {
                    currentTabContent.Destroy?.Invoke();
                    parent.RemoveChild(currentTabContent);
                }

                currentTabContent = CreateContent(argument, this.Left);

                Current = Self;
                OnChange?.Invoke(Self);

                if (currentTabContent is ISceneObjectControl currentTabContentControl)
                {
                    this.AddChild(currentTabContentControl);
                }
                else
                {
                    this.AddChild(currentTabContent);
                }
            }
        }

        /// <summary>
        /// double : left по которому надо расположить элемент контента
        /// </summary>
        protected abstract Func<TArgument, double, TContent> CreateContent { get; }
    }
}