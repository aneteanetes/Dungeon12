using System;
using System.Collections.Generic;
using System.Text;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects.Base;
using Dungeon.View.Interfaces;

namespace Dungeon.SceneObjects
{
    public class MessageBox : DarkRectangle
    {
        public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        public MessageBox(string msg, Action ok)
        {
            this.Width = 16;
            this.Height = 7;
            this.Left = 40d / 2d - 16d / 2d;
            this.Top = 22.5d / 2d - 7d / 2d;

            var question = this.AddTextCenter(msg.AsDrawText().WithWordWrap().Montserrat().InSize(14), true, true);
            question.Width = 15;
            question.Left = 1;
            question.Top -= 2;

            var yesBtn = this.AddControlCenter(new OkButton());
            yesBtn.OnClick = () =>
            {
                this.Destroy?.Invoke();
                ok?.Invoke();
            };

            this.Destroy += () =>
            {
                Global.Freezer.World = null;
            };
        }

        private class OkButton : ButtonControl<EmptySceneObject>
        {
            public OkButton() : base(new EmptySceneObject(), "Ок", 30)
            {
                this.Width = 3;
                this.Height = 2;

                this.AddChild(new DarkRectangle()
                {
                    Color = ConsoleColor.Gray,
                    Width = 3,
                    Height = 2
                });
            }
        }

        public static MessageBox Show(string text, Action ok)
        {
            var msgBox = new MessageBox(text,ok);
            Global.Freezer.World = msgBox;
            Global.SceneManager.CurrentScene.ShowEffectsBinding(msgBox.InList<ISceneObject>());
            return msgBox;
        }
    }
}
