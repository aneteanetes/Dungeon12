namespace Rogue.Scenes.Scenes
{
    using Rogue.Entites.Enemy;

    public class CombatScene : GameScene
    {
        public Enemy Enemy;

        public CombatScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => throw new System.NotImplementedException();

        public override void Draw()
        {
            throw new System.NotImplementedException();
        }
    }
}