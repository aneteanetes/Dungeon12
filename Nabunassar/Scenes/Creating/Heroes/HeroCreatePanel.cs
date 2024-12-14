using Dungeon.Control;
using Dungeon.SceneObjects;
using Nabunassar.Entities;
using Nabunassar.SceneObjects.Base;
using Nabunassar.Scenes.Creating.Character;
using Nabunassar.Scenes.Creating.Character.Stats;

namespace Nabunassar.Scenes.Creating.Heroes
{
    internal class HeroCreatePanel : SceneControl<Hero>
    {
        TextObject plus;
        int _idx = 0;

        HeroPortrait portrait;

        private TextObject race, archetype, fraction, name;

        private TextObject hp;

        private FlatStat ad, ap, arm, bar;

        private FlatStat speed, actp, movp;

        private HeroPrimaryStatValue con, agi, @int, dia;

        protected override ControlEventType[] Handles => new[] { ControlEventType.Click, ControlEventType.Focus };

        private bool _isInteractive;

        private int statTextSize = 25;

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
            agi.Visible = false;

            @int = this.AddChild(new HeroPrimaryStatValue(component, 2));
            @int.Left = agi.LeftMax + 20;
            @int.Top = con.Top;
            @int.Visible = false;

            dia = this.AddChild(new HeroPrimaryStatValue(component, 3));
            dia.Left = @int.LeftMax + 20;
            dia.Top = con.Top;
            dia.Visible = false;

            var statsTopOffset = 10;
            var delimiterTopoffset = 35;

            hp = this.AddText("_".DefaultTxt(statTextSize), fraction.Left, 245);
            hp.Visible = false;


            var leftstatoffset = 15;
            var aaabTop = hp.TopMax + delimiterTopoffset+10;
            ad = this.AddChild(new FlatStat(component, "Icons/Flat/ad.png", () => Component.Offencive.AttackDamage.AsDrawText(statTextSize + 7), Global.Strings["guide"]["AttackDamage"])
            {
                Left = 50,
                Top= aaabTop,
                Visible =false
            });

            ap = this.AddChild(new FlatStat(component, "Icons/Flat/ap.png", () => Component.Offencive.AbilityPower.AsDrawText(statTextSize + 7), Global.Strings["guide"]["AbilityPower"])
            {
                Left = ad.LeftMax + leftstatoffset,
                Top = aaabTop,
                Visible = false
            });

            //arm = this.AddChild(new FlatStat(component, "Icons/Flat/arm.png", () => Component.Offencive.Armor.AsDrawText(statTextSize + 7))
            //{
            //    Left = ap.LeftMax+ leftstatoffset,
            //    Top = aaabTop,
            //    Visible = false
            //});

            //bar = this.AddChild(new FlatStat(component, "Icons/Flat/bar.png", () => Component.Offencive.Barrier.AsDrawText(statTextSize + 7))
            //{
            //    Left = ap.LeftMax + leftstatoffset,
            //    Top = aaabTop,
            //    Visible = false
            //});

            actp = this.AddChild(new FlatStat(component, "Icons/Flat/actp.png", () => Component.MapStats.ActionPoints.AsDrawText(statTextSize + 7), Global.Strings["guide"]["ActionPoints"])
            {
                Left = ap.LeftMax + leftstatoffset,
                Top = aaabTop,
                Visible = false
            });

            movp = this.AddChild(new FlatStat(component, "Icons/Flat/movp.png", () => Component.MapStats.MovementPoints.AsDrawText(statTextSize + 7), Global.Strings["guide"]["MovementPoints"])
            {
                Left = actp.LeftMax + leftstatoffset,
                Top = aaabTop,
                Visible = false
            });

            speed = this.AddChild(new FlatStat(component, "Icons/Flat/speed.png", () => Component.Speed.AsDrawText(statTextSize + 7), Global.Strings["guide"]["Speed"])
            {
                Left = movp.LeftMax + leftstatoffset,
                Top = aaabTop,
                Visible = false
            });

            //actp = this.AddText("_".DefaultTxt(20), con.Left, bar.TopMax + delimiterTopoffset);
            //actp.Visible = false;

            //movp = this.AddText("_".DefaultTxt(20), con.Left, actp.TopMax + statsTopOffset);
            //movp.Visible = false;

            //speed = this.AddText("_".DefaultTxt(20), con.Left, movp.TopMax + statsTopOffset);
            //speed.Visible = false;
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

            con.Visible = agi.Visible = @int.Visible = dia.Visible = hp.Visible = ap.Visible = ad.Visible = /*arm.Visible = bar.Visible =*/ actp.Visible = movp.Visible = speed.Visible = Global.Game.Creation.StatsUnblocked;

            if(Global.Game.Creation.StatsUnblocked)
            {
                Component.BindPersona();
                hp.SetText(Component.Health.AsDrawText(statTextSize,true));
            //    ad.SetText(Component.Offencive.AttackDamage.AsDrawText(true,20));
            //    ap.SetText(Component.Offencive.AbilityPower.AsDrawText(true, 20));
            //    arm.SetText(Component.Offencive.Armor.AsDrawText(true, 20));
            //    bar.SetText(Component.Offencive.Barrier.AsDrawText(true, 20));
            //    actp.SetText(Component.MapStats.ActionPoints.AsDrawText(true, 20));
            //    movp.SetText(Component.MapStats.MovementPoints.AsDrawText(true, 20));
            //    speed.SetText(Component.Speed.AsDrawText(true, 20));
            }

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
