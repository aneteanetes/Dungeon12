namespace Dungeon.Drawing.SceneObjects.Dialogs.NPC
{
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon.Conversations;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.Map.Objects;
    using Dungeon.SceneObjects;
    using Dungeon.SceneObjects.Base;
    using System;
    using System.Linq;

    public class SubjectPanel : DarkRectangle
    {
        public override bool AbsolutePosition => true;

        private ButtonControl exitBtn;
        private Сonversational conv;
        private Action<Subject> select;
        private Action exit;

        public SubjectPanel(Сonversational conv, Action<Subject> select, Action exit, ButtonControl btn)
        {
            this.select = select;
            this.conv = conv;
            this.exit = exit;

            this.Opacity = 0.8;

            this.Top = 0;
            this.Left = 31;

            this.Height = 22.5;
            this.Width = 9;
            
            var txt = this.AddTextCenter(new DrawText(conv.Name), true, false);
            txt.Top = 0.5;

            exitBtn = btn;
            btn.AbsolutePosition = true;
            btn.Top = 19;
            btn.OnClick = ButtonClick;

            exitBtn.Left = this.Width / 2 - exitBtn.Width / 2;

            this.AddChild(exitBtn);

            if (conv.ScreenImage == null)
            {
                SetConversation(conv.Conversations.First(), select, true);
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

        private void ButtonClick()
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
            //exitBtn.SetText("Выход");

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

        private void SetConversation(Conversation conv, Action<Subject> select, bool alone=false)
        {
            if (!alone)
            {
                back = true;
                //exitBtn.SetText("Назад");
            }

            destroyConversation?.Invoke();

            var face = new ImageControl(conv.Face);
            face.Height = 3;
            face.Width = 3;
            face.AbsolutePosition = true;
            face.Top = 2;
            face.Left = this.Width / 2 - face.Width / 2;

            this.AddChild(face);

            destroyConversation += () =>
             {
                 face.Destroy?.Invoke();
                 this.RemoveChild(face);
             };

            if (!alone)
            {
                var txt = this.AddTextCenter(new DrawText(conv.Name).Montserrat(), true);
                txt.Top = 5.5;
                destroyConversation += () =>
                {
                    txt.Destroy?.Invoke();
                    this.RemoveChild(txt);
                };
            }

            double x = 7;

            foreach (var subj in conv.Subjects)
            {
                var subjClick = new SubjectClickable(subj, select);
                subjClick.Top = x;
                subjClick.Left = this.Width / 2 - subjClick.Width / 2;

                this.AddChild(subjClick);
                destroyConversation += () =>
                {
                    subjClick.Destroy?.Invoke();
                    this.RemoveChild(subjClick);
                };

                x += 2.5;
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

        private class ConversactionClickable : HandleSceneControl
        {
            public override bool CacheAvailable => false;

            public override bool AbsolutePosition => true;

            public ConversactionClickable(Сonversational conv, Action<Conversation> select)
            {
                double top = 2;
                this.Width = 9;

                foreach (var con in conv.Conversations)
                {
                    var face = new FaceClickControl(con.Face,con.Name, () => select(con));
                    face.AbsolutePosition = true;
                    face.Top = top;
                    face.Left = this.Width / 2 - face.Width / 2;

                    this.AddChild(face);

                    top += 4.5;
                }

                this.Height = conv.Conversations.Sum(x => 4.5);
            }

            private class FaceClickControl : HandleSceneControl
            {
                public override bool CacheAvailable => false;

                public override bool AbsolutePosition => true;

                private readonly Action onClick;

                private TextControl nameText;

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