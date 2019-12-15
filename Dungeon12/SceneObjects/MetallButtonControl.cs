namespace Dungeon12.Drawing.SceneObjects
{
    using Dungeon;
    using Dungeon.Drawing;
    using Dungeon.GameObjects;
    using Dungeon.SceneObjects.Base;
    public class MetallButtonControl : ButtonControl<EmptyGameComponent>
    {
        public MetallButtonControl(string text) : base(EmptyGameComponent.Empty, text.AsDrawText().Triforce(), 24)
        {
        }
        public MetallButtonControl(DrawText text) : base(EmptyGameComponent.Empty, text, text.Size)
        {
        }

        public override double Width { get => 8.375; set { } }

        public override double Height { get => 2.40625; set { } }

        public override string Image { get; set; } = "Dungeon12.Resources.Images.ui.button.png";

        public override void Focus()
        {
            //Global.AudioPlayer.Effect("focus.wav".AsmSoundRes());
            this.Image = "Dungeon12.Resources.Images.ui.button_f.png";
            //this.textControl.Text.Paint(ActiveColor, true);
        }

        public override void Unfocus()
        {
            this.Image = "Dungeon12.Resources.Images.ui.button.png";
            //this.textControl.Text.Paint(InactiveColor, true);
        }
    }
}