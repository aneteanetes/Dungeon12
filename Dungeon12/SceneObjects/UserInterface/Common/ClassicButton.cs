using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using Nabunassar.SceneObjects.Base;
using System;

namespace Nabunassar.SceneObjects.UserInterface.Common
{
    internal class ClassicButton : EmptySceneControl
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        readonly TextObject Label;

        public ClassicButton(string text, double width=250, double height = 65, int fontSize = 28)
        {
            this.Width = width;
            this.Height = height;


            this.AddBorderMapBack(new BorderConfiguration()
            {
                ImagesPath= "UI/bordermin/bord31.png",
                Size=16,
                Padding = 2
            });

            var txt = text.Gabriela().InColor(Global.CommonColorLight).InSize(fontSize).IsNew(true);

            this.Label= this.AddTextCenter(txt);
            Label.Top -= 5;
            //this.Image="UI/btn_a.png";
        }

        private bool _disabled;
        public bool Disabled
        {
            get => _disabled;
            set
            {
                _disabled = value;
                if (value)
                    Label.Text.ForegroundColor = DrawColor.Gray;
                else
                    Label.Text.ForegroundColor = Global.CommonColorLight;
            }
        }

        public void SetText(string txt)
        {
            Label.Text.SetText(txt);
        }

        public Action OnClick { get; set; }

        public override void Click(PointerArgs args)
        {
            if (Disabled)
                return;
            
            OnClick?.Invoke();
            base.Click(args);
        }

        public override void Focus()
        {
            if (Disabled)
                return;

            AudioPlayer.Sound("focus.wav".AsmSoundRes());
            Label.Text.ForegroundColor = Global.CommonColor;
        }

        public override void Unfocus()
        {
            if (Disabled)
                return;

            Label.Text.ForegroundColor = Global.CommonColorLight;
        }
    }
}
