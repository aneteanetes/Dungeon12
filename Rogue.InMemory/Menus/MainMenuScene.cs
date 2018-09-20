namespace Rogue.InMemory.Menus
{
    using Rogue.InMemory.Character;
    using Rogue.InMemory.Scenes;

    public class MainMenuScene : Scene<CreateScene>
    {
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Draw()
        {
            throw new System.NotImplementedException();
        }
    }
}
