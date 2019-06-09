namespace Rogue.Scenes.Menus.Creation
{
    using Rogue.Control.Keys;
    using Rogue.Drawing.SceneObjects;
    using Rogue.Drawing.SceneObjects.Dialogs;
    using Rogue.Scenes.Manager;

    public class PlayerOriginScene : GameScene<PlayerSummaryScene, PlayerRaceScene>
    {
        public PlayerOriginScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Init()
        {
            this.AddObject(new ImageControl("Rogue.Resources.Images.d12back.png"));
            this.AddObject(new OriginDialogue(this.AddControl, this.RemoveControl)
            {
                Top = 3f,
                Left = 7f,
                OnSelect = o =>
                {
                    PlayerAvatar.Character.Origin = o;
                    this.Switch<PlayerSummaryScene>();
                }
            });
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape && !hold)
            {
                this.Switch<PlayerRaceScene>();
            }
        }
    }
}
