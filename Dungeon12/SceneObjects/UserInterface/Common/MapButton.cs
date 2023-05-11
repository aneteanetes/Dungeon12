﻿using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using System;

namespace Dungeon12.SceneObjects.UserInterface.Common
{
    internal class MapButton : EmptySceneControl
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        readonly TextObject Label = null;

        public MapButton()
        {
            this.Width = 234;
            this.Height = 59;

            //Label = this.AddTextCenter("Выбрать".AsDrawText().Triforce().InSize(24));
        }

        private bool _disabled;
        public bool Disabled
        {
            get => _disabled;
            set
            {
                _disabled = value;
                //if (txt)
                //    Label.Strings.ForegroundColor = DrawColor.Gray;
                //else
                //    Label.Strings.ForegroundColor = DrawColor.White;
            }
        }

        public void SetText(string txt)
        {
            Label.Text.SetText(txt);
        }

        public Action OnClick { get; set; }

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
        }

        public override string Image { get; set; } = "Backgrounds/mapbtn.png".AsmImg();

        public override void Focus()
        {
            if (Disabled)
                return;

            Image = "Backgrounds/mapbtn_l.png".AsmImg();
        }

        public override void Unfocus()
        {
            if (Disabled)
                return;

            Image = "Backgrounds/mapbtn.png".AsmImg();
        }
    }
}