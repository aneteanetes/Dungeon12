using Dungeon.Control;
using Dungeon.SceneObjects;
using Nabunassar.Entities;
using Nabunassar.SceneObjects.Base;
using Nabunassar.Scenes.Creating.Character;

namespace Nabunassar.Scenes.Creating.Heroes
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
            _isInteractive = isInteractive;
            _idx = idx;
            Width = 425;
            Height = 700;

            this.AddBorderMapBack(new BorderConfiguration()
            {
                ImagesPath = "UI/bordermin/panel-border-022.png",
                Size = 16,
                Padding = 2
            });

            plus = AddTextCenter("+".Navieo().InSize(300).InColor(Global.CommonColorLight));
            plus.Visible = component == null;

            portrait = AddChild(new HeroPortrait(component)
            {
                Left = 25,
                Top = 25
            });

            var topOffset = 35;
            var leftOffsetCut = 215;
            var leftOffset = leftOffsetCut - 30;
            var statTextSize = 25;

            name = AddText(Global.Strings["Unknown"].ToString().DefaultTxt(statTextSize), leftOffsetCut, 15);
            race = AddText(Global.Strings["Race"].ToString().DefaultTxt(statTextSize), leftOffsetCut, name.Top + topOffset);
            archetype = AddText(Global.Strings["Archetype"].ToString().DefaultTxt(statTextSize), leftOffset, race.Top + topOffset);
            fraction = AddText(Global.Strings["Fraction"].ToString().DefaultTxt(statTextSize), leftOffset, archetype.Top + topOffset);
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
                Switch<CreateHeroScene>();
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
            name.Visible = Component?.Race != null;
            archetype.Visible = Component?.Archetype != null;
            fraction.Visible = Component?.Fraction != null;

            if (Component != null)
            {
                if (Component.Race != null)
                    race.SetText(Global.Strings["Race"] + " : " + Global.Strings[Component.Race.ToString()]);

                if (Component.Archetype != null)
                    archetype.SetText(Global.Strings["Archetype"] + " : " + Global.Strings[Component.Archetype.ToString()]);

                if (Component.Fraction != null)
                    fraction.SetText(Global.Strings["Fraction"] + " : " + Global.Strings[Component.Fraction.ToString()]);

            }
        }
    }
}
