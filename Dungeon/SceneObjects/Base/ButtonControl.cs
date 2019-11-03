using Dungeon.Control;
using Dungeon.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.SceneObjects.Base
{
    public class ButtonControl : HandleSceneControl
    {
        protected TextControl textControl;

        public ButtonControl(string text, float size=30)
        {
            textControl = new TextControl(new DrawText(text, ConsoleColor.White) { Size = size }.Triforce());

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

        public Action OnClick { get; set; }

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
        }
    }
}
