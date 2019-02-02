namespace Rogue.Scenes.Controls
{
    using Rogue.View.Interfaces;
    using System;

    public class ButtonControl : HandleSceneControl
    {
        private TextControl textControl;

        public ButtonControl(IDrawText text)
        {
            textControl = new TextControl(text);

            textControl.Left += 0.55;
            textControl.Top += 0.55;

            this.Children.Add(textControl);
        }

        public IDrawColor ActiveColor { get; set; }

        public IDrawColor InactiveColor { get; set; }

        public Action OnClick { get; set; }

        public override string Image { get; set; } = "Rogue.Resources.Images.ui.button.png";

        public override void Click()
        {
            OnClick?.Invoke();
        }

        public override void Focus()
        {
            this.Image= "Rogue.Resources.Images.ui.button_f.png"; 
            //this.textControl.Text.Paint(ActiveColor, true);
        }

        public override void Unfocus()
        {
            this.Image = "Rogue.Resources.Images.ui.button.png";
            //this.textControl.Text.Paint(InactiveColor, true);
        }
    }
}
