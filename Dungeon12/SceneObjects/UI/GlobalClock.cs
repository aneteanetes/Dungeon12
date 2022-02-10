using Dungeon;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.SceneObjects.Base;
using System;

namespace Dungeon12.SceneObjects.UI
{
    public class GlobalClock : SceneControl<Calendar>
    {
        public GlobalClock(Calendar component) : base(component, true)
        {
            this.Width = 125;
            this.Height = 125;
            Image = "clock.png".AsmImg();
            this.AddChild(tooltip = new ClockTooltip(component));
        }

        GameTimeLoop prev;

        public override void UpdateSceneObject(GameTimeLoop gameTime)
        {
            if (prev.TotalGameTime == default)
            {
                prev = gameTime;
                return;
            }

            var diff = gameTime.TotalGameTime - prev.TotalGameTime;

            if (diff.TotalSeconds >= 2)
            {
                Component.Add(0, 1);
                prev = gameTime;
            }

            base.UpdateSceneObject(gameTime);
        }

        private ClockTooltip tooltip;

        public override void Focus()
        {
            tooltip.SetPosition(new Point(125, 125));
            tooltip.Visible = true;

            base.Focus();
        }

        public override void Unfocus()
        {
            tooltip.Visible = false;
            base.Unfocus();
        }

        private class ClockTooltip : DarkRectangle
        {
            public override bool Filtered => false;

            public override bool Interface => true;

            public IDrawText TooltipText => Text;

            Calendar calendar;

            public ClockTooltip(Calendar calendar)
            {
                this.calendar = calendar;
                Opacity = 0.8;

                var textSize = MeasureText(calendar.ClockText().AsDrawText().Gabriela());

                Width = textSize.X + 10;
                Height = textSize.Y + 5;
            }

            public override IDrawText Text => calendar.ClockText().AsDrawText().Gabriela();

            public void SetPosition(Point position)
            {
                Left = position.X;
                Top = position.Y;
            }
        }
    }
}