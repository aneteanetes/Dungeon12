using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using System;

namespace Dungeon12.Scenes.Menus.MainMenu
{
    public class MainMenuButton : EmptySceneControl
    {
        private string Source(bool focus = false) => $"GUI/Buttons/MainMenuBtn{(focus ? "_focus" : "")}.png".AsmImg();

        public MainMenuButton(string text)
        {
            this.Image = Source();

            this.Width = 300;
            this.Height = 96;

            this.AddTextCenter(text.AsDrawText()
                .Carribean()
                .InSize(42)
                .InColor(DrawColor.WhiteSmoke));
        }

        public override void Focus()
        {
            Global.AudioPlayer.Effect("focus.wav".AsmSoundRes());
            this.Image = Source(true);
        }

        public override void Unfocus()
        {
            this.Image = Source();
        }

        public Action OnClick;

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
            base.Click(args);
        }
    }
}