using Dungeon;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;

namespace Dungeon12.Scenes.Start
{
    internal class StartScene : StartScene<MenuScene>
    {
        public StartScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        Loading loading;

        public override void Initialize()
        {
            DungeonGlobal.GameClient.SetCursor("Cursors/common.png".AsmImg());

            var layerBack = CreateLayer("back");
            layerBack.AddObjectCenter(loading = new Loading());

            this.PreLoad<MenuScene>();
        }

        public override void Load()
        {
            this.Resources.LoadFont("NAVIEO Trial");
        }

        public override void Update(GameTimeLoop gameTimeLoop)
        {
            if(loading.Counter>=1)
                this.Switch<MenuScene>();

            base.Update(gameTimeLoop);
        }
    }
}
