using Dungeon;

namespace Dungeon12.SceneObjects.NPC
{
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon12.Conversations;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon12.Map.Objects;
    using Dungeon12.SceneObjects;
    using Dungeon.SceneObjects;
    using Dungeon.SceneObjects.Base;
    using Dungeon12.SceneObjects.Base;
    using Dungeon12.Drawing.SceneObjects;
    using System;
    using System.Linq;
    using Dungeon12;

    public class SubjectPanel : EmptyHandleSceneControl
    {
        public override bool AbsolutePosition => true;

        private MetallButtonControl exitBtn;
        private Сonversational conv;
        private Action<Subject> select;
        private Action exit;

        public SubjectPanel(Сonversational conv, Action<Subject> select, Action exit, MetallButtonControl btn)
        {
            this.select = select;
            this.conv = conv;
            this.exit = exit;

            //Opacity = 0.8;

            Image = "ui/dialogs/subjectpanel.png".AsmImgRes();

            Top = 0;
            Left = 31;

            Height = 22.5;
            Width = 9;

            var txt = AddTextCenter(new DrawText(conv.Name), true, false);
            txt.Top = 0.5;

            exitBtn = btn;
            btn.AbsolutePosition = true;
            btn.Top = 19;
            btn.OnClick = ButtonClick;

            exitBtn.Left = Width / 2 - exitBtn.Width / 2;

            this.AddChild(exitBtn);

            if (conv.ScreenImage == null)
            {
                SetConversation(conv.Conversations.FirstOrDefault(), select, true);
            }
            else
            {
                SetFaces();
            }
        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.Escape)
            {
                ButtonClick();
            }
        }

        public void ButtonClick()
        {
            if (back)
            {
                SetFaces();
            }
            else
            {
                exit?.Invoke();
            }
        }

        protected override Key[] KeyHandles => new Key[] { Key.Escape };

        private Action destroyConversation;

        private ConversactionClickable conversactionClickable;

        private bool back = false;

        private void SetFaces()
        {
            back = false;
            destroyConversation?.Invoke();

            conversactionClickable?.Destroy?.Invoke();
            this.RemoveChild(conversactionClickable);

            conversactionClickable = new ConversactionClickable(conv, x =>
            {
                conversactionClickable?.Destroy.Invoke();
                this.RemoveChild(conversactionClickable);
                SetConversation(x, select);
            });

            this.AddChild(conversactionClickable);
        }

        private void SetConversation(Conversation conv, Action<Subject> select, bool alone = false)
        {
            if (conv == null)
            {
                destroyConversation?.Invoke();

                var staticface = new ImageControl(this.conv.FaceImage) { Blur = true };
                staticface.Height = 3;
                staticface.Width = 3;
                staticface.AbsolutePosition = true;
                staticface.Top = 2;
                staticface.Left = Width / 2 - staticface.Width / 2;

                AddChild(staticface);

                destroyConversation += () =>
                {
                    staticface.Destroy?.Invoke();
                    RemoveChild(staticface);
                };

                return;
            }

            if (!alone)
            {
                back = true;
                //exitBtn.SetText("Назад");
            }

            destroyConversation?.Invoke();

            var face = new ImageControl(conv.Face) { Blur = true };
            face.Height = 3;
            face.Width = 3;
            face.AbsolutePosition = true;
            face.Top = 2;
            face.Left = Width / 2 - face.Width / 2;

            AddChild(face);

            destroyConversation += () =>
             {
                 face.Destroy?.Invoke();
                 RemoveChild(face);
             };

            if (!alone)
            {
                var txt = AddTextCenter(new DrawText(conv.Name).Montserrat(), true);
                txt.Top = 5.5;
                destroyConversation += () =>
                {
                    txt.Destroy?.Invoke();
                    RemoveChild(txt);
                };
            }

            double x = 7;

            var subjects = conv.Subjects.Where(s => (s.Visible?.Name == default || !s.Visible.Triggered) && (s.Invisible?.Name == default || s.Invisible.Triggered));
            foreach (var subj in subjects)
            {
                var subjClick = new SubjectClickable(subj, select);
                subjClick.Top = x;
                subjClick.Left = Width / 2 - subjClick.Width / 2;

                this.AddChild(subjClick);
                destroyConversation += () =>
                {
                    subjClick.Destroy?.Invoke();
                    this.RemoveChild(subjClick);
                };

                x += 2.5;
            }
        }

        private class SubjectClickable : EmptyHandleSceneControl
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
                subj = subject;

                this.Width = MeasureText(txt).X / 32;
                this.Height = 1;
            }

            public override void Focus()
            {
                this.Text.ForegroundColor = new DrawColor(ConsoleColor.Yellow);
                base.Focus();
            }

            public override void Unfocus()
            {
                this.Text.ForegroundColor = new DrawColor(ConsoleColor.White);
                base.Unfocus();
            }

            public override void Click(PointerArgs args)
            {
                select?.Invoke(subj);
            }
        }

        private class ConversactionClickable : EmptyHandleSceneControl
        {
            public override bool CacheAvailable => false;

            public override bool AbsolutePosition => true;

            public ConversactionClickable(Сonversational conv, Action<Conversation> select)
            {
                double top = 2;
                this.Width = 9;

                foreach (var con in conv.Conversations.Where(x => Global.GameState.Character[x.Id + "DELETED"] == default))
                {
                    var face = new FaceClickControl(con.Face, con.Name, () => select(con));
                    face.AbsolutePosition = true;
                    face.Top = top;
                    face.Left = this.Width / 2 - face.Width / 2;

                    this.AddChild(face);

                    top += 4.5;
                }

                this.Height = conv.Conversations.Sum(x => 4.5);
            }

            private class FaceClickControl : EmptyHandleSceneControl
            {
                public override bool CacheAvailable => false;

                public override bool AbsolutePosition => true;

                private readonly Action onClick;

                private TextControl nameText;

                public override bool Blur => true;

                public FaceClickControl(string img, string name, Action onClick)
                {
                    this.onClick = onClick;
                    this.Image = img;

                    this.Height = 3;
                    this.Width = 3;

                    var text = new DrawText(name).Montserrat();
                    nameText = this.AddTextCenter(text, true);
                    nameText.Left = this.Width / 2 - MeasureText(text).X / 32 / 2;
                    nameText.Top = 3.5;
                }

                public override void Click(PointerArgs args) => onClick?.Invoke();

                public override void Focus()
                {
                    nameText.Text.ForegroundColor = new DrawColor(ConsoleColor.Yellow);
                    base.Focus();
                }

                public override void Unfocus()
                {
                    nameText.Text.ForegroundColor = new DrawColor(ConsoleColor.White);
                    base.Unfocus();
                }
            }
        }
    }
}