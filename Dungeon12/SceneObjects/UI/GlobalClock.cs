﻿using Dungeon;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using Dungeon.SceneObjects.Base;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Nabunassar.ECS.Components;
using Nabunassar.Entities;
using Nabunassar.SceneObjects.Base;
using System;

namespace Nabunassar.SceneObjects.UI
{
    internal class GlobalClock : SceneControl<Calendar>
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public override bool PerPixelCollision => true;

        public GlobalClock(Calendar component) : base(component)
        {
            this.Width = 250;
            this.Height = 90;
            Image = "clock.back.png".AsmImg();
            this.AddChild(new ClockFace(component)
            {
                Left = 60,
                Top = -75
            });
        }

        GameTimeLoop prev;

        private class ClockFace : SceneControl<Calendar>
        {
            public override void Throw(Exception ex)
            {
                throw ex;
            }

            public ClockFace(Calendar calendar) : base(calendar)
            {
                this.Width = 125;
                this.Height = 125;
                this.Image = "clock.png".AsmImg();
                this.AddChild(tooltip = new ClockTooltip(calendar));
            }

            private ClockTooltip tooltip;

            public override void Focus()
            {
                tooltip.SetPosition(new Dot(125, 125));
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

                public void SetPosition(Dot position)
                {
                    Left = position.X;
                    Top = position.Y;
                }
            }
        }

        public override void Update(GameTimeLoop gameTime)
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

            base.Update(gameTime);
        }
    }
}