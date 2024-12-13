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
        private HeroPrimaryStatValue con, agi, @int, dia;

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

            con = this.AddChild(new HeroPrimaryStatValue(component, 0));
            con.Left = fraction.Left;
            con.Top = fraction.TopMax + 60;
            con.Visible = false;

            agi = this.AddChild(new HeroPrimaryStatValue(component, 1));
            agi.Left = con.LeftMax+20;
            agi.Top = con.Top;
            con.Visible = false;

            @int = this.AddChild(new HeroPrimaryStatValue(component, 2));
            @int.Left = agi.LeftMax + 20;
            @int.Top = con.Top;
            con.Visible = false;

            dia = this.AddChild(new HeroPrimaryStatValue(component, 3));
            dia.Left = @int.LeftMax + 20;
            dia.Top = con.Top;
            con.Visible = false;


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
            con.Visible = agi.Visible = @int.Visible = dia.Visible = Global.Game.Creation.StatsUnblocked;

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
