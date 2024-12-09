using Dungeon.SceneObjects;
using Dungeon12.Entities;
using Dungeon12.SceneObjects.Base;
using System.Drawing.Imaging;

namespace Dungeon12.Scenes.Creating.Character
{
    internal class HeroCreatePanel : SceneControl<Hero>
    {
        TextObject plus;

        public HeroCreatePanel(Hero component) : base(component)
        {
            this.Width = 400;
            this.Height = 700;

            this.AddBorderMapBack(new BorderConfiguration()
            {
                ImagesPath = "UI/bordermin/panel-border-022.png",
                Size = 16,
                Padding = 2
            });

            plus = this.AddTextCenter("+".Navieo().InSize(300).InColor(Global.CommonColorLight));
            plus.Visible = component == null;
        }

        string prevCursor = null;

        public override void Focus()
        {
            DungeonGlobal.GameClient.SetCursor("Cursors/hand_thin_open.png".AsmImg());
        }

        public override void Unfocus()
        {
            DungeonGlobal.GameClient.SetCursor("Cursors/pointer_scifi_b.png".AsmImg());
        }

        public void BindHero(Hero component)
        {
            plus.Visible = component == null;
        }
    }
}
