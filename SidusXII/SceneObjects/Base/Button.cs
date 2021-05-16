using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using System;

namespace SidusXII.SceneObjects.Base
{
    public class Button : EmptySceneControl
    {
        TextControl centerText;
        public Button(string text)
        {
            Width = 350;
            Height = 50;

            centerText = AddTextCenter(text.AsDrawText()
                .InSize(42)
                .InColor(DrawColor.WhiteSmoke));
        }

        private bool _disabled;
        public bool Disabled
        {
            get => _disabled;
            set
            {
                _disabled = value;
                if (value)
                    centerText.Text.ForegroundColor = DrawColor.Gray;
            }
        }

        public override void Focus()
        {
            if (Disabled)
                return;

            //DungeonGlobal.AudioPlayer.Effect("focus.wav".AsmSoundRes());
            centerText.Text.ForegroundColor = DrawColor.LightGoldenrodYellow;
        }

        public override void Unfocus()
        {
            if (Disabled)
                return;

            centerText.Text.ForegroundColor = DrawColor.White;
        }

        public Action OnClick;

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
            base.Click(args);
        }
    }
}