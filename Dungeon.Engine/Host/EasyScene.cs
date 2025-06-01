using Dungeon.Scenes;
using Dungeon.Scenes.Manager;

namespace Dungeon.Engine.Host
{
    public class EasyScene : GameScene
    {
        public override bool Destroyable => true;

        public override bool AbsolutePositionScene => false;

        public EasyScene(SceneManager sceneManager) : base(sceneManager) { }

        public override void Initialize()
        {
        }
    }
}
