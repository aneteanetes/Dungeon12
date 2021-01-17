using Dungeon.Scenes;
using Dungeon.Scenes.Manager;

namespace Dungeon.Scenes
{
    public class @Sys_Clear_Screen : StartScene
    {
        public Sys_Clear_Screen(SceneManager sceneManager) : base(sceneManager) { }

        public override bool Destroyable => true;

        public override void Initialize() { }
    }
}
