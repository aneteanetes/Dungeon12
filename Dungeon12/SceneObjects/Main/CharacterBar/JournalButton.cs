namespace Dungeon12.Drawing.SceneObjects.Main.CharacterBar
{
    using Dungeon.Control.Keys;
    using Dungeon12.Drawing.SceneObjects.Main.CharacterInfo;
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Map;
    using Dungeon.View.Interfaces;
    using System;
    using Dungeon;
    using Dungeon.Drawing.SceneObjects;
    using System.Collections.Generic;
    using Dungeon12.SceneObjects;
    using Dungeon.Control;
    using Dungeon12.Entities.Quests;
    using Dungeon.Scenes.Manager;

    public class JournalButton : SlideComponent
    {
        public override bool AbsolutePosition => true;
        public override bool CacheAvailable => false;

        private Player playerSceneObject;

        public JournalButton(Player playerSceneObject) : base("Журнал (L)")
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

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold) => ShowJournalBtn();

        public override void Click(PointerArgs args) => ShowJournalBtn();

        private static JournalWindow jWindow = null;

        public static void OpenQuest(IQuest quest)
        {
            if (jWindow == null)
            {
                Global.GameState.Player.StopMovings();
                jWindow = new JournalWindow(Global.GameState.Player.As<Player>(),quest);
                jWindow.Destroy += () => jWindow = null;

                SceneManager.Current.ShowEffectsBinding(jWindow.InList<ISceneObject>());
            }
        }

        private void ShowJournalBtn()
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