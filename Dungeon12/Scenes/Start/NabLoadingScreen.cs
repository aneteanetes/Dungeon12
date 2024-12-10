using Dungeon;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;

namespace Nabunassar.Scenes.Start
{
    internal class NabLoadingScreen : LoadingScreen
    {
        public NabLoadingScreen(SceneManager sceneManager, Action onLoaded) : base(sceneManager, onLoaded)
        {
        }

        Loading loading;

        protected override bool IsNeedWaitUntilLoad => true;

        public override void Initialize()
        {
            DungeonGlobal.GameClient.SetCursor(Resources, "Cursors/pointer_scifi_b.png".AsmImg());

            var layerBack = CreateLayer("back");
            layerBack.AddObjectCenter(loading = new Loading(Strings["Loading"]));
        }

        public override void Load()
        {
            this.Resources.LoadFont("NAVIEO Trial");
            Resources.LoadGlobal("Cursors/pointer_scifi_b.png".AsmImg());
            Resources.LoadGlobal("Cursors/hand_thin_open.png".AsmImg());
        }

        public override void Update(GameTimeLoop gameTimeLoop)
        {
            if (loading.Counter >= 1)
                LoadComplete();

            base.Update(gameTimeLoop);
        }
    }
}
