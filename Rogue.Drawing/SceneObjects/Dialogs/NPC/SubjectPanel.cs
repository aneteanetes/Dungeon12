namespace Rogue.Drawing.SceneObjects.Dialogs.NPC
{
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects.Base;

    public class SubjectPanel : DarkRectangle
    {
        public override bool AbsolutePosition => true;

        public SubjectPanel(Rogue.Map.Objects.NPC npc)
        {
            this.Opacity = 0.8;

            this.Top = 0;
            this.Left = 31;

            this.Height = 22.5;
            this.Width = 9;

            
            var txt = this.AddTextCenter(new DrawText(npc.Name), true, false);
            txt.Top = 0.5;

            var face = new ImageControl(npc.Face);
            face.Height = 3;
            face.Width = 3;
            face.AbsolutePosition = true;
            face.Top = 2;
            face.Left = this.Width/2 - face.Width / 2;

            this.AddChild(face);
        }
    }
}
