using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Console
{
    /// <summary>
    /// Interface: Button; Clickable;
    /// </summary>
    public class Button : Interface
    {
        public Button(Window Window)
        {
            this.AutoClear = false;
            this.Parent = Window;
            this.OnFocus = () => { this.Active = true; };
        }

        public bool Border = true;

        public override bool Activatable => true;

        public Action OnClick;
        private List<List<ColouredChar>> constructed = new List<List<ColouredChar>>();
        public override IEnumerable<IDrawText> Construct(bool Active)
        {
            this.Paths.Clear();
            this.WanderingText.Clear();

            this.DrawRegion = new Types.Rectangle
            {
                X = this.Parent.Left + this.Left,
                Y = this.Parent.Top + this.Top,
                Width = this.Width,
                Height = this.Height
            };

            this.Paths.Add(new DrawablePath
            {
                ForegroundColor = ColorSchemaColor,
                Fill = false,
                Angle = 10,
                Depth = 5f,
                PathPredefined = View.Enums.PathPredefined.Rectangle,
                Region = new Types.Rectangle
                {
                    X = this.DrawRegion.X * 24,
                    Y = this.DrawRegion.Y * 24,
                    Width = this.DrawRegion.Width * 24,
                    Height = this.DrawRegion.Height * 24
                }
            });

            var lableColor = Active ? this.ActiveColor : this.InactiveColor;
            var labelText = new DrawText(this.Label, lableColor);
            var label = new Label(this)
            {
                SourceText = labelText,
                Align = TextPosition.Left,
                Left = this.Width / 2 - this.Label.Length/2 / 2,
                Top = this.Height / 2 - 1 / 2,
            };

            var labels = label.Construct(Active);


            this.WanderingText.AddRange(labels);

            return labels;

            //var color = Active
            //    ? this.ActiveColor
            //    : this.InactiveColor;

            //var top = new DrawText((Border ? Window.Border.UpperLeftCorner : ' ') + GetLine((int)this.Width - 2, (Border ? Window.Border.HorizontalLine : ' ')) + (Border ? Window.Border.UpperRightCorner : ' '), Window.BorderColor);

            //var mid = DrawText.Empty((int)this.Width,Window.BorderColor);
            //mid.ReplaceAt(0, new DrawText((Border ? Window.Border.VerticalLine.ToString() : " "), Window.BorderColor));
            //mid.ReplaceAt(1, new DrawText(this.Middle(this.Label), color));
            //mid.ReplaceAt((int)this.Width - 1, new DrawText((Border ? Window.Border.VerticalLine.ToString() : " "), Window.BorderColor));

            //var bot = new DrawText((Border ? Window.Border.LowerLeftCorner : ' ') + GetLine((int)this.Width - 2, (Border ? Window.Border.HorizontalLine : ' ')) + (Border ? Window.Border.LowerRightCorner : ' '), Window.BorderColor);

            //return new IDrawText[] { top, mid, bot };
        }

        public override IDrawSession Run()
        {
            return base.Run();
        }
    }
}
