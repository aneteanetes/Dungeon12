namespace Dungeon12.Drawing.SceneObjects.Main.CharacterBar
{
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon12.Drawing.SceneObjects.Main.CharacterInfo;
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Map;
    using Dungeon.View.Interfaces;
    using System;using Dungeon;using Dungeon.Drawing.SceneObjects;
    using System.Collections.Generic;

    public class TalantsButton : SlidesafeComponent
    {
        public override bool AbsolutePosition => true;
        public override bool CacheAvailable => false;

        private PlayerSceneObject playerSceneObject;
        private GameMap gamemap;

        public TalantsButton(PlayerSceneObject playerSceneObject, Action<List<ISceneObject>> showEffects) : base("Таланты (X)", showEffects)
        {
            this.playerSceneObject = playerSceneObject;

            this.Height = 1;
            this.Width = 1;

            this.AddChild(new ImageControl("Dungeon.Resources.Images.ui.player.tal.png")
            {
                CacheAvailable = false,
                Height = 1,
                Width = 1,
            });

            this.Image = SquareTexture(false);
        }

        private string SquareTexture(bool focus)
        {
            var f = focus
                ? "_f"
                : "";

            return $"Dungeon.Resources.Images.ui.square{f}.png";
        }

        public override void Focus()
        {
            this.Image = SquareTexture(true);
            base.Focus();
        }

        public override void Unfocus()
        {
            this.Image = SquareTexture(false);
            base.Unfocus();
        }

        protected override Key[] KeyHandles => new Key[]
        {
            Key.X
        };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold) => ShowTalWindow();

        public override void Click(PointerArgs args) => ShowTalWindow();

        private TalantWindow talWindow = null;

        private void ShowTalWindow()
        {
            if (talWindow != null)
            {
                talWindow.Destroy?.Invoke();
                return;
            }            

            playerSceneObject.StopMovings();

            talWindow = new TalantWindow(playerSceneObject);
            talWindow.Destroy += () => talWindow = null;

            this.ShowEffects(new List<ISceneObject>()
            {
                talWindow
            });
        }

    }
}