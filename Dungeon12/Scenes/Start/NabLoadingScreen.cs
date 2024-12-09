using Dungeon;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;

namespace Dungeon12.Scenes.Start
{
    internal class NabLoadingScreen : LoadingScreen
    {
        public NabLoadingScreen(SceneManager sceneManager, Action onLoadComplete) : base(sceneManager, onLoadComplete)
        {
        }

        Loading loading;

        public override void Initialize()
        {
            DungeonGlobal.GameClient.SetCursor("Cursors/common.png".AsmImg());

            var layerBack = CreateLayer("back");
            layerBack.AddObjectCenter(loading = new Loading());
        }

        public override void Load()
        {
            this.Resources.LoadFont("NAVIEO Trial");
        }

        public override void Update(GameTimeLoop gameTimeLoop)
        {
            if (loading.Counter >= 1)
                LoadComplete();

            base.Update(gameTimeLoop);
        }
    }
}
