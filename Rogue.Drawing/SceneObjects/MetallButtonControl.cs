namespace Rogue.Drawing.SceneObjects
{
    using Rogue.Control.Pointer;
    using Rogue.Drawing.Impl;
    using Rogue.Settings;
    using Rogue.View.Interfaces;
    using System;

    public class MetallButtonControl : HandleSceneControl
    {
        private TextControl textControl;

        public MetallButtonControl(string text)
        {
            textControl = new TextControl(new DrawText(text, ConsoleColor.White) { Size = 30 });

            var measure = Global.DrawClient.MeasureText(textControl.Text);

            var width = this.Width * 32;
            var height = this.Height * 32;
            
            var left = width / 2 - measure.X / 2;
            var top = height / 2 - measure.Y / 2;

            //left /= 1.8f;

            textControl.Left = left / 32;
            textControl.Top = top / 32;

            this.Children.Add(textControl);
        }

        public void SetText(string txt)
        {
            this.textControl.Text.SetText(txt);
        }

        public override double Width { get => 8.375; set { } }
        public override double Height { get => 2.40625; set { } }

        public Action OnClick { get; set; }

        public override string Image { get; set; } = "Rogue.Resources.Images.ui.button.png";

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
        }

        public override void Focus()
        {
            Global.AudioPlayer.Effect("focus");
            this.Image = "Rogue.Resources.Images.ui.button_f.png";
            //this.textControl.Text.Paint(ActiveColor, true);
        }

        public override void Unfocus()
        {
            this.Image = "Rogue.Resources.Images.ui.button.png";
            //this.textControl.Text.Paint(InactiveColor, true);
        }
    }
}