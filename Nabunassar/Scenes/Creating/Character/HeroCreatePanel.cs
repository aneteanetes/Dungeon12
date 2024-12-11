using Dungeon.Control;
using Dungeon.SceneObjects;
using Nabunassar.Entities;
using Nabunassar.SceneObjects.Base;
using Nabunassar.Scenes.Creating.Heroes;

namespace Nabunassar.Scenes.Creating.Character
{
    internal class HeroCreatePanel : SceneControl<Hero>
    {
        TextObject plus;
        int _idx = 0;

        protected override ControlEventType[] Handles => new[] { ControlEventType.ClickRelease, ControlEventType.Focus };

        public HeroCreatePanel(Hero component, int idx) : base(component)
        {
            _idx= idx;
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

        public override void ClickRelease(PointerArgs args)
        {
            Global.Game.Creation.CharacterCreationPosition = _idx;
            SetCursor(SceneObjects.Cursors.Cursor.Normal);
            this.Switch<CreateHeroScene>();
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
