namespace Rogue.InMemory.Scenes
{
    public class CharacterScene : Scene
    {

        public SceneTab PopUpTab;

        public SceneTab iTab;

        public SceneTab MerchTab;

        public SceneTab Qtab;
    }

    public class SceneTab
    {
        public string Type;

        public int NowTab;

        public int MaxTab;
    }
}