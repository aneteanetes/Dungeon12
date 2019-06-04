namespace Rogue.Drawing.SceneObjects.Dialogs.NPC
{
    using Rogue.Drawing.SceneObjects.Base;

    public class AnswerPanel : DarkRectangle
    {
        public override bool AbsolutePosition => true;

        public AnswerPanel()
        {
            this.Opacity = 0.8;

            this.Top = 15;
            this.Left = 0;

            this.Height = 7.5;
            this.Width = 31;
        }
    }
}