using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;

namespace Dungeon12.Scenes
{
    internal class StartScene : StartScene<LoadingScene>
    {
        public StartScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Initialize()
        {
            Global.GameClient.SetCursor("Cursors/common.png".AsmImg());

            var layerBack = this.CreateLayer("back");
            layerBack.AddObject(new ImageObject("loadingscreen.png".AsmImg())
            {
                Width = Global.Resolution.Width,
                Height = Global.Resolution.Height
            });
        }

        public override void Loaded()
        {
            this.Switch<LoadingScene>();
        }
    }
}
