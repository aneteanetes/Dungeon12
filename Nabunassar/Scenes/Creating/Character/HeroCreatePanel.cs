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

        HeroPortrait portrait;

        private TextObject race, archetype, fraction, name;

        protected override ControlEventType[] Handles => new[] { ControlEventType.Click, ControlEventType.Focus };

        private bool _isInteractive;

        public HeroCreatePanel(Hero component, int idx, bool isInteractive = false) : base(component)
        {
            _isInteractive=isInteractive;
            _idx = idx;
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

            portrait = this.AddChild(new HeroPortrait(component)
            {
                Left = 25,
                Top = 25
            });

            name = this.AddText(Global.Strings["Unknown"].ToString().DefaultTxt(25), 215, 15);
            race = this.AddText(Global.Strings["Race"].ToString().DefaultTxt(25), 215, 50);
            archetype = this.AddText(Global.Strings["Archetype"].ToString().DefaultTxt(25), 190, 85);

        }

        public override void Focus()
        {
            if (_isInteractive)
                SetCursor(SceneObjects.Cursors.Cursor.Pointer);
        }

        public override void Click(PointerArgs args)
        {
            if (_isInteractive)
            {
                Global.Game.Creation.CharacterCreationPosition = _idx;
                SetCursor(SceneObjects.Cursors.Cursor.Normal);
                this.Switch<CreateHeroScene>();
            }
        }

        public override void Unfocus()
        {
            if (_isInteractive)
                SetCursor(SceneObjects.Cursors.Cursor.Normal);
        }

        public void BindHero(Hero component)
        {
            plus.Visible = component == null;
        }



        public override bool Updatable => true;

        public override void Update(GameTimeLoop gameTime)
        {
            portrait.Visible = Component?.Race != null;
            race.Visible = Component?.Race != null;
            name.Visible = Component?.Name != null;
            archetype.Visible = Component?.Archetype != null;

            if (Component != null)
            {
                if (Component.Race != null)
                    race.SetText(Global.Strings["Race"] + " : " + Global.Strings[Component.Race.ToString()]);

                if (Component.Archetype != null)
                    archetype.SetText(Global.Strings["Archetype"] + " : " + Global.Strings[Component.Archetype.ToString()]);
            }
        }
    }
}
