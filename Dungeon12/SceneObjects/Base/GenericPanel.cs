using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Entities.Plates;
using Dungeon12.Extensions.SceneObjects;

namespace Dungeon12.SceneObjects.Base
{
    internal class GenericPanel : SceneControl<GenericData>
    {
        public GenericPanel(GenericData component) : base(component)
        {
            this.AddChild(new Icon(component.Icon)
            {
                Left=-72
            });

            var top = 7d;
            var left = 7d;
            var topAdd = 22;
            var offsetTop = topAdd-20; //20 - font size

            var list = new ListR<IDrawText>();

            var name = list.Add(component.Title());

            var subtype = list.Add(component.Subtype());

            var rank = component.Rank();
            var res = component.Resources();
            var radius = component.Radius();

            if (res!=null || radius!=null)
                list.Add(res ?? radius);

            var dur = list.Add(component.Duration());
            var cd = list.Add(component.Cooldown());

            if (dur!=null || cd!=null)
                list.Add(dur ?? cd);

            var charge = list.Add(component.Charges());
            var requires = list.Add(component.Requires());
            var level = list.Add(component.RequireLevel());
            var fraction = list.Add(component.Fraction());
            var text = list.Add(component.Description().WithWordWrap());
            var rune = list.Add(component.Rune());

            if (rank==null)
            {
                this.Width = this.MeasureText(name).X + 100;
            }
            else
            {
                var firstWidth = this.MeasureText(name).X + 50 + this.MeasureText(rank).X;
                this.Width = firstWidth;
                if (cd!=null)
                {
                    var durcdWidth = this.MeasureText(cd).X + 50 + this.MeasureText(dur).X;
                    if (durcdWidth>firstWidth)
                        this.Width=durcdWidth;
                }
            }

            if (component.SizeSettings!=default)
            {
                if (component.SizeSettings.Width>0 && this.Width<component.SizeSettings.Width)
                    this.Width = component.SizeSettings.Width;
            }

            this.Height = this.MeasureText(name).Y+5;

            var border = this.AddBorderBack(.99);

            double Right(IDrawText drawText)
            {
                return this.Width-this.MeasureText(drawText).X - 10;
            }

            this.AddText(name, left, top);

            if (rank != null)
                this.AddText(rank, Right(rank), top);

            if (subtype!=null)
            {
                top+=topAdd+5;
                this.AddText(subtype, left, top);
                this.Height += this.MeasureText(subtype).Y+offsetTop;
            }

            if (res!=null || radius!=null)
            {
                top+=topAdd+5;
                this.AddText(res ?? radius, left, top);
                this.Height += this.MeasureText(res ?? radius).Y+offsetTop;
                if (res!=null && radius!=null)
                    this.AddText(radius, Right(radius), top);
            }

            if (dur!=null || cd!=null)
            {
                top+=topAdd;
                this.AddText(dur ?? cd, left, top);
                this.Height += this.MeasureText(dur ?? cd).Y+offsetTop;
                if (dur!=null && cd!=null)
                    this.AddText(cd, Right(cd), top);
            }

            if (charge!=null)
            {
                top+=topAdd;
                this.AddText(charge, left, top);
                this.Height += this.MeasureText(charge).Y+offsetTop;
            }

            if (requires!=null)
            {
                top+=topAdd;
                this.AddText(requires, left, top);
                this.Height += this.MeasureText(charge).Y+offsetTop;
            }

            if (level!=null)
            {
                top+=topAdd;
                this.AddText(level, left, top);
                this.Height += this.MeasureText(requires).Y + offsetTop;
            }

            if (fraction!=null)
            {
                top+=topAdd;
                this.AddText(fraction, left, top);
                this.Height += this.MeasureText(fraction).Y + offsetTop;
            }

            this.Height +=10;

            TextObject description = null;
            if (text!=null)
            {
                top+=topAdd+7;
                description = this.AddText(text, left, top);
                description.Width = this.Width;
                description.Height = this.MeasureText(text, this).Y;
                top+=description.Height+topAdd+7;
                this.Height+=description.Height+topAdd+7;
            }

            if (rune!=null)
            {
                this.AddText(rune, left, top);
                this.Height += this.MeasureText(rune).Y;
            }

            border.Resize(this.Width, this.Height);
        }

        private class Icon : ImageObject
        {
            public Icon(string iconAbs)
            {
                this.Width=69;
                this.Height=69;

                this.AddBorder();

                var margin = 5;

                this.AddChild(new ImageObject(iconAbs)
                {
                    Width=this.Width-margin*2,
                    Height=this.Height-margin*2,
                    Top=margin,
                    Left=margin
                });
            }
        }
    }
}