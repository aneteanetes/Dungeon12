namespace Rogue.Drawing.Controls
{
    using System;
    using System.Collections.Generic;
    using Rogue.Control.Events;
    using Rogue.Drawing.Impl;
    using Rogue.Types;
    using Rogue.View.Interfaces;

    public abstract class BaseControl : DrawSession, IDrawable
    {
        public BaseControl()
        {
            this.AutoClear = false;
        }

        private BaseControl container;

        private List<BaseControl> innerComponents = new List<BaseControl>();

        public virtual string Icon { get; set; }

        public virtual string Name { get; set; }

        public abstract string Tileset { get; }

        public abstract Rectangle TileSetRegion { get; }

        public virtual IDrawColor BackgroundColor { get; set; }

        public virtual IDrawColor ForegroundColor { get; set; }

        public float Height { get; set; }

        public float Width { get; set; }

        public float Left { get; set; }

        public float Top { get; set; }

        public override Rectangle SessionRegion => this.Region;

        public Rectangle Region
        {
            get
            {
                return new Rectangle
                {
                    X = this.Left,
                    Y = this.Top,
                    Width = this.Width,
                    Height = this.Height
                };
            }
            set { throw new System.Exception("can't set region for control"); }
        }

        public virtual bool Container => false;

        public void Append(BaseControl another)
        {
            innerComponents.Add(another);
        }

        public override IDrawSession Run()
        {
            foreach (var item in innerComponents)
            {
                item.container = this;
                item.Left += this.Left;
                item.Top += this.Top;
                
                item.Run();
            }

            this.Drawables = new IDrawable[] { this };

            return base.Run();
        }

        public Action OnFocus;

        public Action OnUnfocus;

        public Action OnClick;

        public override void Handle(ControlEventType @event)
        {
            if (!this.IsControlable)
                return;

            switch (@event)
            {
                case ControlEventType.Click:
                    this.OnClick?.Invoke();
                    break;
                case ControlEventType.Focus:
                    this.OnFocus?.Invoke();
                    break;
                case ControlEventType.Unfocus:
                    this.OnUnfocus?.Invoke();
                    break;
                default:
                    break;
            }
        }

        public override void Publish()
        {
            base.Publish();

            foreach (var component in innerComponents)
            {
                component.Publish();
            }

            innerComponents = new List<BaseControl>();
        }
    }
}