using Dungeon.SceneObjects;
using Nabunassar.SceneObjects.Base;

namespace Nabunassar.Scenes.Creating.Heroes
{
    internal class CreateDescWindow : EmptySceneControl
    {
        public override bool Updatable => true;

        private TextObject text;

        public CreateDescWindow() : base()
        {
            this.Width = 400;
            this.Height = 700;

            this.AddBorderMapBack(new BorderConfiguration()
            {
                ImagesPath = "UI/bordermin/panel-border-022.png",
                Size = 16,
                Padding = 2
            });

            var title = this.AddTextCenter(Global.Strings["Description"].ToString().DefaultTxt(22));

            title.Top = 20;

            text = this.AddTextCenter("".DefaultTxt(18,true));
            text.Width = 350;
            text.Height = 600;
            text.Left = 25;
            text.Top = 64;
        }

        private string hint = "";

        public override void Update(GameTimeLoop gameTime)
        {
            if (hint != Global.Game.Creation.Hint)
            {
                hint = Global.Game.Creation.Hint;
                text.SetText(hint);
            }

            base.Update(gameTime);
        }
    }
}
