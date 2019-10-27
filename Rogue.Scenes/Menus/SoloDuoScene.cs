namespace Rogue.Scenes.Menus
{
    using Rogue.Control.Keys;
    using Rogue.Drawing.SceneObjects;
    using Rogue.Drawing.SceneObjects.Dialogs;
    using Rogue.Scenes.Manager;
    using Rogue.Scenes.Menus.Creation;

    public class SoloDuoScene : GameScene<PlayerNameScene,NetworkScene, Start>
    {
        public SoloDuoScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;
        
        public override void Init()
        {
            this.AddObject(new Background(true));
            this.AddObject(new ImageControl("Rogue.Resources.Images.d12textM.png")
            {
                Top = 2f,
                Left = 10f
            });

            this.AddObject(new MetallButtonControl("Один игрок")
            {
                Left = 15.5f,
                Top = 8,
                OnClick = () => { this.Switch<PlayerNameScene>(); }
            });

            this.AddObject(new MetallButtonControl("Сеть")
            {
                Left = 15.5f,
                Top = 11,
                OnClick = () => { this.Switch<NetworkScene>(); }
            });
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
