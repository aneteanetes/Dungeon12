namespace Rogue.Drawing.SceneObjects
{
    using Rogue.Control.Pointer;
    using Rogue.Drawing.Impl;
    using Rogue.Settings;
    using Rogue.View.Interfaces;
    using System;

    public class SmallMetallButtonControl : HandleSceneControl
    {
        private TextControl textControl;

        public SmallMetallButtonControl(string text)
        {
            textControl = new TextControl(new DrawText(text, ConsoleColor.White) { Size = 30 });

            var measure = Global.DrawClient.MeasureText(textControl.Text);

            var width = this.Width * 32;
            var height = this.Height * 32;
            
            var left = width / 2 - measure.X / 2;
            var top = height / 2 - measure.Y / 2;
            
            textControl.Left = left / 32;
            textControl.Top = top / 32;

            this.Children.Add(textControl);
        }

        public SmallMetallButtonControl(IDrawText text)
        {
            textControl = new TextControl(text);

            var measure = Global.DrawClient.MeasureText(textControl.Text);

            var width = this.Width * 32;
            var height = this.Height * 32;

            var left = width / 2 - measure.X / 2;
            var top = height / 2 - measure.Y / 2;

            textControl.Left = left / 32;
            textControl.Top = top / 32;

            this.Children.Add(textControl);
        }

        public override double Width { get => 4.8125; set { } }
        public override double Height { get => 2.40625; set { } }

        public Action OnClick { get; set; }

        public override string Image { get; set; } = "Rogue.Resources.Images.ui.button_s.png";

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
        }

        public override void Focus()
        {
            this.Image = "Rogue.Resources.Images.ui.button_s_f.png";
        }

        public override void Unfocus()
        {
            this.Image = "Rogue.Resources.Images.ui.button_s.png";
        }
    }
}