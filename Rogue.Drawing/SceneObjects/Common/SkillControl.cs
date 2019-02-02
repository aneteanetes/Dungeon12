namespace Rogue.Drawing.SceneObjects.Common
{
    using System;

    public class SkillControl : HandleSceneControl
    {

        public override double Width { get => 2; set { } }
        public override double Height { get => 2; set { } }

        public Action OnClick { get; set; }

        public override string Image { get; set; } = "Rogue.Resources.Images.ui.square.png";

        public override void Click()
        {
            OnClick?.Invoke();
        }

        public override void Focus()
        {
            this.Image = "Rogue.Resources.Images.ui.square_f.png";
        }

        public override void Unfocus()
        {
            this.Image = "Rogue.Resources.Images.ui.square.png";
        }
    }
}