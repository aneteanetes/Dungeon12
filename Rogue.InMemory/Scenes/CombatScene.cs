namespace Rogue.InMemory.Scenes
{
    using Rogue.Entites.Enemy;

    public class CombatScene : GameScene
    {
        public Enemy Enemy;

        public CombatScene(SceneManager sceneManager) : base(sceneManager)
        {
        }
    }
}