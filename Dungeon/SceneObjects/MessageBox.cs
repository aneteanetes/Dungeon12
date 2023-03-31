using Dungeon.Control.Keys;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects.Base;
using Dungeon.View.Interfaces;
using System;
using System.Runtime.CompilerServices;

namespace Dungeon.SceneObjects
{
    public class MessageBox : DarkRectangle
    {
        public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        Action ok;

        public MessageBox(string msg, Action ok)
        {
            this.ZIndex = int.MaxValue - 5;
            this.ok = ok;

            this.Width = 16;
            this.Height = 7;
            this.Left = 40d / 2d - 16d / 2d;
            this.Top = 22.5d / 2d - 7d / 2d;

            var question = this.AddTextCenter(msg.AsDrawText().WithWordWrap().InSize(14), true, true);
            question.Width = 15;
            question.Left = 1;
            question.Top -= 2;

            var yesBtn = this.AddControlCenter(new OkButton(),true);
            yesBtn.Top = 5.5;
            yesBtn.Left = 8;
            yesBtn.OnClick = () =>
            {
                this.Destroy();
                ok?.Invoke();
            };
        }

        public override void Destroy()
        {
            DungeonGlobal.Freezer.World = null;
            base.Destroy();
        }

        private class OkButton : ButtonControl<EmptySceneObject>
        {
            public OkButton() : base(new EmptySceneObject(), "Ок", 30)
            {
                this.Width = 3;
                this.Height = 2;

                this.AddChild(new DarkRectangle()
                {
                    Color = new DrawColor(ConsoleColor.Gray),
                    Width = 3,
                    Height = 2,
                    Top=-1,
                    Left=-1.5
                });
            }
        }

        protected override Key[] KeyHandles => new Key[] { Key.Escape };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.Escape)
            {
                if(ok!=default)
                {
                    ok();
                }
                else
                {
                    DungeonGlobal.SceneManager.Start("FATAL");
                }
            }

        }

        public static MessageBox Show(string text, Action ok)
        {
            var msgBox = new MessageBox(text,ok);
            DungeonGlobal.Freezer.World = msgBox;            
            DungeonGlobal.SceneManager.CurrentScene.ShowEffectsBinding(msgBox.InList<ISceneObject>());
            return msgBox;
        }
    }
}
