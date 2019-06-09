namespace Rogue.Drawing.SceneObjects.Main.CharacterBar
{
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.SceneObjects.Main.CharacterInfo;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public class CharButton : TooltipedSceneObject
    {
        public override bool AbsolutePosition => true;

        private PlayerSceneObject playerSceneObject;
        private Action<List<ISceneObject>> showEffects;

        public CharButton(PlayerSceneObject playerSceneObject, Action<List<ISceneObject>> showEffects) : base("Персонаж", showEffects)
        {
            this.playerSceneObject = playerSceneObject;
            this.showEffects = showEffects;

            this.Height = 1.5;
            this.Width = 1.5;

            this.AddChild(new ImageControl("Rogue.Resources.Images.ui.player.character.png")
            {
                CacheAvailable = true,
                Height = 1.5,
                Width = 1.5,
            });

            this.Image = SquareTexture(false);
        }

        private string SquareTexture(bool focus)
        {
            var f = focus
                ? "_f"
                : "";

            return $"Rogue.Resources.Images.ui.square{f}.png";
        }

        public override void Focus()
        {
            this.Image = SquareTexture(true);
            base.Focus();
        }

        public override void Unfocus()
        {
            this.Image = SquareTexture(false);
            base.Focus();
        }

        protected override Key[] KeyHandles => new Key[]
        {
            Key.C
        };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold) => ShowInfo();

        public override void Click(PointerArgs args) => ShowInfo();

        private CharacterInfoWindow characterInfoWindow;

        private void ShowInfo()
        {
            if (characterInfoWindow != null)
                return;

            playerSceneObject.StopMovings();

            characterInfoWindow = new CharacterInfoWindow(playerSceneObject, showEffects);
            characterInfoWindow.Destroy += () => characterInfoWindow = null;

            this.ShowEffects(new List<ISceneObject>()
            {
                characterInfoWindow
            });
        }
    }
}