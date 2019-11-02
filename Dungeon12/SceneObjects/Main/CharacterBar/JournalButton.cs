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
    using Dungeon12.SceneObjects;

    public class JournalButton : SlidesafeComponent
    {
        public override bool AbsolutePosition => true;
        public override bool CacheAvailable => false;

        private Player playerSceneObject;

        public JournalButton(Player playerSceneObject, Action<List<ISceneObject>> showEffects) : base("Журнал (L)", showEffects)
        {
            this.playerSceneObject = playerSceneObject;

            this.Height = 1;
            this.Width = 1;

            this.AddChild(new ImageControl("Dungeon12.Resources.Images.ui.player.journal.png")
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

            return $"Dungeon12.Resources.Images.ui.square{f}.png";
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
            Key.L
        };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold) => ShowTalWindow();

        public override void Click(PointerArgs args) => ShowTalWindow();

        private JournalWindow jWindow = null;

        private void ShowTalWindow()
        {
            if (jWindow != null)
            {
                jWindow.Destroy?.Invoke();
                return;
            }            

            playerSceneObject.StopMovings();

            jWindow = new JournalWindow(playerSceneObject);
            jWindow.Destroy += () => jWindow = null;

            this.ShowEffects(new List<ISceneObject>()
            {
                jWindow
            });
        }

    }
}