using Dungeon.SceneObjects;
using Nabunassar.Entities;
using Nabunassar.SceneObjects.Base;

namespace Nabunassar.Scenes.Creating.Character
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

        public override void Focus()
        {
            SetCursor(SceneObjects.Cursors.Cursor.Pointer);
        }

        public override void Unfocus()
        {
            SetCursor(SceneObjects.Cursors.Cursor.Normal);
        }

        public void BindHero(Hero component)
        {
            plus.Visible = component == null;
        }
    }
}
