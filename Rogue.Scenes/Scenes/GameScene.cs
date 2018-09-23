namespace Rogue.Scenes.Scenes
{
    using Rogue.Entites.Alive.Character;
    using Rogue.Logging;
    using Rogue.Map;

    public class GameScene : Scene
    {
        public Player Player;

        public Location Map;

        public Logger Log;

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        public override void Draw()
        {
            throw new System.NotImplementedException();
        }
    }
}