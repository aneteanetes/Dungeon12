namespace Dungeon12.Scenes.Menus.Creation
{
    using Dungeon.Control.Keys;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.Drawing.SceneObjects.Dialogs;
    using Dungeon.Scenes.Manager;

    public class PlayerNameScene : GameScene<PlayerRaceScene,Start>
    {
        public PlayerNameScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;
        
        public override void Init()
        {
            this.AddObject(new ImageControl("Rogue.Resources.Images.d12back.png"));

            this.AddObject(new TypeNameDialogue(Next,Back)
            {
                Top = 3f,
                Left = 14f,
            });
        }

        private void Next(string value)
        {
            if (this.PlayerAvatar == null)
            {
                this.PlayerAvatar = new Map.Objects.Avatar(new Rogue.Classes.Noone.Noone());
            }

            this.PlayerAvatar.Character.Name = value[0].ToString().ToUpper() + value.Substring(1);

            this.Switch<PlayerRaceScene>();
        }

        private void Back()
        {
            this.Switch<Start>();
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape && !hold)
            {
                this.Switch<Start>();
            }
        }
    }
}
