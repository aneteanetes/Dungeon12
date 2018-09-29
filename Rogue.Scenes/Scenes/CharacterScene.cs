namespace Rogue.Scenes.Scenes
{
    public class CharacterScene : GameScene
    {
        public SceneTab PopUpTab;

        public SceneTab iTab;

        public SceneTab MerchTab;

        public SceneTab Qtab;

        public CharacterScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => throw new System.NotImplementedException();

        public override void Draw()
        {
            //base.Draw();
        }
    }

    public class SceneTab
    {
        public string Type;

        public int NowTab;

        public int MaxTab;
    }
}