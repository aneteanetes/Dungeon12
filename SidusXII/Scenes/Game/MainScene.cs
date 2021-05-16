using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using SidusXII.Scenes.MainMenu;

namespace SidusXII.Scenes.Game
{
    public class MainScene : GameScene<MainMenuScene>
    {
        public MainScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        public override void Initialize()
        {
        }
    }
}