namespace Dungeon12.Scenes.Menus.Creation
{
    using Dungeon.Control.Keys;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon12.Map.Objects;
    using Dungeon.Scenes;
    using Dungeon.Scenes.Manager;
    using Dungeon12.Drawing.SceneObjects.Dialogs;
    using Dungeon12.Abilities;

    public class PlayerNameScene : GameScene<PlayerOriginScene, SoloDuoScene>
    {
        public PlayerNameScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;
        
        public override void Initialize()
        {
            this.AddObject(new ImageObject("Dungeon12.Resources.Images.d12back.png"));

            this.AddObject(new TypeNameDialogue(Next,Back)
            {
                Top = 3f,
                Left = 14f,
            });
        }

        private void Next(string value)
        {
            Cooldown.ResetAll();
            var character = new Noone.Noone(true);
            character.Name = value[0].ToString().ToUpper() + value.Substring(1);
            Global.GameState.Character = character;
            Global.GameState.PlayerAvatar = new Avatar(character);

            this.Switch<PlayerOriginScene>();
        }

        private void Back()
        {
            this.Switch<SoloDuoScene>();
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape && !hold)
            {
                this.Switch<SoloDuoScene>();
            }
        }
    }
}
