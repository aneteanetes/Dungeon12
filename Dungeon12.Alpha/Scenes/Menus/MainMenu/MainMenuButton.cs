using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using System;

namespace Dungeon12.Scenes.Menus.MainMenu
{
    public class MainMenuButton : EmptySceneControl
    {
        TextControl centerText;
        public MainMenuButton(string text)
        {
            this.Width = 350;
            this.Height = 50;

            centerText = this.AddTextCenter(text.AsDrawText()
                .Carribean()
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
                    this.centerText.Text.ForegroundColor = DrawColor.Gray;
            }
        }

        public override void Focus()
        {
            if (Disabled)
                return;

            Global.AudioPlayer.Effect("focus.wav".AsmSoundRes());
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