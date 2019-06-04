namespace Rogue.Drawing.SceneObjects.Dialogs.NPC
{
    using Rogue.Control.Pointer;
    using Rogue.Conversations;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects.Base;
    using System;

    public class SubjectPanel : DarkRectangle
    {
        public override bool AbsolutePosition => true;

        public SubjectPanel(Rogue.Map.Objects.NPC npc, Action<Subject> select)
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

            double x = 7;

            foreach (var subj in npc.NPCEntity.Conversation.Subjects)
            {
                var subjClick = new SubjectClickable(subj, select);
                subjClick.Top = x;
                subjClick.Left = this.Width / 2 - subjClick.Width / 2;

                this.AddChild(subjClick);

                x+=2.5;
            }
        }

        private class SubjectClickable : HandleSceneControl
        {
            public override bool AbsolutePosition => true;
            public override bool CacheAvailable => false;

            private Action<Subject> select;
            private Subject subj;

            public SubjectClickable(Subject subject, Action<Subject> select)
            {
                this.select = select;
                DrawText txt = subject.Name;
                this.Text = txt;
                this.subj = subject;

                this.Width = MeasureText(txt).X / 32;
                this.Height = 1;
            }

            public override void Focus()
            {
                this.Text.ForegroundColor = new DrawColor(System.ConsoleColor.Yellow);
                base.Focus();
            }

            public override void Unfocus()
            {
                this.Text.ForegroundColor = new DrawColor(System.ConsoleColor.White);
                base.Unfocus();
            }

            public override void Click(PointerArgs args)
            {
                select?.Invoke(subj);
            }
        }
    }
}
