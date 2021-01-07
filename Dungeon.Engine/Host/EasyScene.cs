using Dungeon.Scenes;
using Dungeon.Scenes.Manager;

namespace Dungeon.Engine.Host
{
    public class EasyScene : StartScene
    {
        public override bool Destroyable => true;

        public override bool AbsolutePositionScene => false;

        public EasyScene(SceneManager sceneManager) : base(sceneManager) { }

        public override void Init()
        {
            base.Init();
        }
    }
}
