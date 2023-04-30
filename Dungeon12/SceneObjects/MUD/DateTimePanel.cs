using Dungeon;
using Dungeon.SceneObjects;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;
using System;
using System.ComponentModel;

namespace Dungeon12.SceneObjects.MUD
{
    internal class DateTimePanel : SceneControl<Calendar>
    {
        private TextObject time;

        public DateTimePanel(Calendar component) : base(component)
        {
            this.Width=400;
            this.Height=30;

            this.AddBorder();

            var shim = this.AddTextCenter("Мерцание: ".AsDrawText().InSize(18).InBold().Calibri(), vertical: true);
            shim.Left=5;
            this.AddChild(new MonthView(component) { Left = 95, Top = 5 });
            
            time = this.AddTextCenter(component.TimeText().AsDrawText().InSize(18).InBold().Calibri());

            this.AddChild(new DateView(component)
            {
                Left = this.Width-25,
                Top=5
            });

            this.AddChildCenter(new DayView(component)
            {
                Left= this.Width-100
            }, horizontal: false);
        }

        public override void Update(GameTimeLoop gameTime)
        {
            time.Text.SetText(Component.TimeText());
            base.Update(gameTime);
        }

        private class MonthView : SceneControl<Calendar>, ITooltiped
        {
            public MonthView(Calendar component) : base(component)
            {
                this.Width = 20;
                this.Height = 20;
            }

            public override string Image => $"Icons/MUD/months/{((MonthYear)Component.Month)}.tga".AsmImg();

            public string TooltipText => ((MonthYear)Component.Month).AsShimmer();
        }

        private class DateView : SceneControl<Calendar>, ITooltiped
        {
            public DateView(Calendar component):base(component)
            {
                this.Width = 20;
                this.Height = 20;

                this.Image = $"Icons/MUD/calendar.tga".AsmImg();
            }

            public string TooltipText => Component.DateText();
        }

        private class DayView : SceneControl<Calendar>
        {
            public DayView(Calendar component) : base(component)
            {
                this.Width=100;

                this.Text=component.DayOfWeek.ToDisplay().AsDrawText().InSize(18).InBold().Calibri();

                this.Height = this.MeasureText(this.Text).Y;
            }

            public override void Update(GameTimeLoop gameTime)
            {
                this.Text.SetText(Component.DayOfWeek.ToDisplay());
                base.Update(gameTime);
            }
        }
    }
}