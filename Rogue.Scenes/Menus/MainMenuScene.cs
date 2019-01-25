namespace Rogue.Scenes.Menus
{
    using System;
    using System.Linq;
    using Rogue.Races.Perks;
    using Rogue.Scenes.Menus.Creation;
    using Rogue.Scenes.Scenes;

    public class MainMenuScene : GameScene<PlayerNameScene, Game.MainScene>
    {
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Render()
        {
            
        }
    }
}