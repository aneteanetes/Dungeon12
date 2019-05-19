namespace Rogue.Map.Editor
{
    using Rogue.Drawing.SceneObjects;
    using Rogue.Scenes;
    using Rogue.Scenes.Manager;

    public class EditorScene : GameScene
    {
        public EditorScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        public override void Init()
        {
            this.AddObject(new Background());
        }
    }
}
