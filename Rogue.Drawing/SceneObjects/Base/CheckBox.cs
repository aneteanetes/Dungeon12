namespace Rogue.Drawing.SceneObjects.Base
{
    using Rogue.Control.Pointer;
    using Rogue.View.Interfaces;
    using System;

    public class CheckBox : HandleSceneControl
    {
        public override bool AbsolutePosition => true;
        public override bool CacheAvailable => false;
        public bool Value = false;

        public Action<bool> OnChange { get; set; }

        private string Img => $"Rogue.Resources.Images.ui.checkbox{(Value ? "_f" : "")}.png";

        public CheckBox(IDrawText drawText)
        {
            var label = new CheckBoxLabel(drawText, Click);
            label.Left += 0.6;
            Image = Img;
            this.Height = 0.5;
            this.Width = 0.5;
            this.AddChild(label);
        }

        public override void Click(PointerArgs args)
        {
            this.Value = !Value;
            OnChange?.Invoke(this.Value);
            Image = Img;
        }

        private class CheckBoxLabel : HandleSceneControl
        {
            private readonly Action<PointerArgs> click;

            public CheckBoxLabel(IDrawText text, Action<PointerArgs> click)
            {
                this.click = click;
                this.Text = text;
            }

            public override void Click(PointerArgs args) => click(args);
        }
    }
}