using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Control.Keys;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Console
{
    /// <summary>
    /// Interface: TextBox; Writeable;
    /// </summary>
    public class TextBox : Interface
    {
        public TextBox(Window Window)
        {
            this.Parent = Window;
            this.DrawRegion = new Types.Rectangle
            {
                X = this.Left,
                Y = this.Top,
                Width = this.Width,
                Height = this.Height
            };

            this.OnFocus = () => { this.Active = true; };
        }

        public Action OnEndTyping;

        public override bool Activatable => true;

        private List<Key> NotAllowed = new List<Key>
        {
            Key.LeftShift,
            Key.RightShift,
            Key.LeftCtrl,
            Key.RightCtrl
        };

        public void OnKeyPress(KeyArgs args)
        {
            if (args.Key == Key.Enter)
            {
                this.OnEndTyping();
                return;
            }

            if (args.Key == Key.Back)
            {
                this.String = this.String.Substring(0, this.String.Length - 1);
            }
            else if (!NotAllowed.Contains(args.Key))
            {
                var symbol = args.Key.ToString();
                if (args.Modifiers != KeyModifiers.Shift)
                    symbol = symbol.ToLowerInvariant();

                this.String += symbol;
            }
            else
            {
                return;
            }

            this.Run();
            this.Publish();
        }

        public string String = "";
        public string Text
        {
            get
            {
                return String;
            }
        }

        public override IDrawSession Run()
        {
            var y = 0;
            var lines = this.Construct(this.Active);
            foreach (var line in lines)
            {
                this.Write(y, 0, line);
                y++;
            }

            return base.Run();
        }

        public override IEnumerable<IDrawText> Construct(bool Active)
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = this.Parent.Left + this.Left,
                Y = this.Parent.Top + this.Top,
                Width = this.Width,
                Height = this.Height
            };

            var color = Active
                ? this.ActiveColor
                : this.InactiveColor;


            string printf = "";
            if (String != "")
            {
                if (String.Length >= this.Width - 2)
                {
                    printf = String.Substring(0, (int)this.Width - 2);
                }
                else
                {
                    printf = String;
                }
            }
            else
            {
                printf = this.Label;
            }

            var top = new DrawText(Additional.LightBorder.UpperLeftCorner + GetLine((int)this.Width - 2, Additional.LightBorder.HorizontalLine) + Additional.LightBorder.UpperRightCorner, Parent.BorderColor);

            var mid = DrawText.Empty((int)this.Width, Parent.BorderColor);
            mid.ReplaceAt(0, new DrawText(Additional.LightBorder.VerticalLine.ToString(), Parent.BorderColor));
            mid.ReplaceAt(1, new DrawText(printf + GetLine((int)this.Width - 2 - printf.Length, ' '), color));
            mid.ReplaceAt((int)this.Width - 1, new DrawText(Additional.LightBorder.VerticalLine.ToString(), Parent.BorderColor));

            var bot = new DrawText(Additional.LightBorder.LowerLeftCorner + GetLine((int)this.Width - 2, Additional.LightBorder.HorizontalLine) + Additional.LightBorder.LowerRightCorner, Parent.BorderColor);

            return new IDrawText[] { top, mid, bot };
        }
    }
}
