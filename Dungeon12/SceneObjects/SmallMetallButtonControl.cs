namespace Dungeon12.Drawing.SceneObjects
{
    using Dungeon;
    using Dungeon.Control;
    using Dungeon.Drawing;
    using Dungeon.SceneObjects;
    using Dungeon.View.Interfaces;
    using System;

    public class SmallMetallButtonControl : EmptyHandleSceneControl
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
            var textControl = this.AddTextCenter(text);
            textControl.Top -= 0.25;
        }

        public override double Width { get => 4.8125; set { } }
        public override double Height { get => 2.40625; set { } }

        public Action OnClick { get; set; }

        public override string Image { get; set; } = "Dungeon12.Resources.Images.ui.button_s.png";

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
        }

        public override void Focus()
        {
            this.Image = "Dungeon12.Resources.Images.ui.button_s_f.png";
        }

        public override void Unfocus()
        {
            this.Image = "Dungeon12.Resources.Images.ui.button_s.png";
        }
    }
}