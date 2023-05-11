using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Localization;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;
using Dungeon12.Entities.Plates;
using Dungeon12.SceneObjects.Base;
using Dungeon12.SceneObjects.Stats;

namespace Dungeon12.SceneObjects.MUD.Info
{
    internal class CharacterInfo : ActiveHeroControl
    {
        ClassBadge classBadge;

        public CharacterInfo()
        {
            Width = 400-NineSliceSettings.Default.Size;
            Height = 800-NineSliceSettings.Default.Size;

            this.AddChild(new HeroName() {  Top=10});
            this.AddChildCenter(new InfoAvatar()
            {
                Top = 50
            }, vertical: false);

            classBadge = this.AddChild(new ClassBadge(Component.Archetype)
            {
                Top = 88,
                Left=50
            });

            this.AddChild(new SpecBadge()
            {
                Top=85,
                Left=295
            });

            this.AddChild(new StatObject(x=>new GenericData()
            {
                Title = "Level".Localized(),
                Icon="Icons/stats/level.tga",
                Rank = "Stat".Localized(),
                Text = "Description.Level".Localized()

            }, x => x.Level.ToString())
            {
                Left = 50,
                Top=230
            });

            this.AddChild(new StatObject(x=>new GenericData()
            {
                Title = "Exp".Localized(),
                Icon="Icons/stats/exp.tga",
                Rank = "Stat".Localized(),
                Text = "Description.Exp".Localized()

            }, x => x.ExpStr())
            {
                Left = 180,
                Top=230
            });

            this.AddChild(new StatObject(x => new GenericData()
            {
                Title = "Damage".Localized(),
                Subtype = x.DamageType.Localized(),
                Icon=$"Icons/stats/attacks/{x.DamageType}.tga",
                Rank = "Stat".Localized(),
                Text = "Description.Damage".Localized()

            }, x => x.Damage.ToString("-"))
            {
                Left = 35,
                Top=305
            });

            this.AddChild(new StatObject(x => new GenericData()
            {
                Title = "CritChance".Localized(),
                Icon=$"Icons/stats/crit.tga",
                Rank = "Stat".Localized(),
                Text = "Description.CritChance".Localized()

            }, x => x.CritChance.ToString(postfix:"%"))
            {
                Left = 160,
                Top=305
            });

            this.AddChild(new StatObject(x => new GenericData()
            {
                Title = "Accuracy".Localized(),
                Icon=$"Icons/stats/accuracy.tga",
                Rank = "Stat".Localized(),
                Text = "Description.Accuracy".Localized()

            }, x => x.Accuracy.ToString(postfix: "%"))
            {
                Left = 275,
                Top=305
            });

            this.AddChild(new StatObject(x => new GenericData()
            {
                Title = "AD".Localized(),
                Icon=$"Icons/stats/ad.tga",
                Rank = "Stat".Localized(),
                Text = "Description.AD".Localized()

            }, x => x.AD.ToString())
            {
                Left = 55,
                Top=385
            });

            this.AddChild(new StatObject(x => new GenericData()
            {
                Title = "Initiative".Localized(),
                Icon=$"Icons/stats/initiative.tga",
                Rank = "Stat".Localized(),
                Text = "Description.Initiative".Localized()

            }, x => x.Initiative.ToString())
            {
                Left = 170,
                Top=385
            });

            this.AddChild(new StatObject(x => new GenericData()
            {
                Title = "AP".Localized(),
                Icon=$"Icons/stats/ap.tga",
                Rank = "Stat".Localized(),
                Text = "Description.AP".Localized()

            }, x => x.AP.ToString())
            {
                Left = 270,
                Top=385
            });

            this.AddChild(new StatObject(x => new GenericData()
            {
                Title = "Armor".Localized(),
                Icon=$"Icons/stats/armor.tga",
                Rank = "Stat".Localized(),
                Text = "Description.Armor".Localized()

            }, x => x.Armor.ToString())
            {
                Left = 55,
                Top=465
            });

            this.AddChild(new StatObject(x => new GenericData()
            {
                Title = x.ArmorType.Localized(),
                Icon=$"Icons/stats/armortypes/{x.ArmorType}.tga",
                Rank = "ArmorType".Localized(),
                Text = Global.Strings.Description[x.ArmorType]
            }, x =>"")
            {
                Left = 180,
                Top=465
            });

            this.AddChild(new StatObject(x => new GenericData()
            {
                Title = "Spellreflect".Localized(),
                Icon=$"Icons/stats/spellreflect.tga",
                Rank = "Stat".Localized(),
                Text = Global.Strings.Description["Spellreflect"]
            }, x => x.SpellReflect.ToString(postfix:"%"))
            {
                Left = 265,
                Top=465
            });
        }

        public override void Update(GameTimeLoop gameTime)
        {
            classBadge.Set(Component.Archetype);
        }

        public override Hero Component => Global.Game.Party.Active;

        public override void Click(PointerArgs args) { }

        class StatObject : ActiveHeroControl
        {

            private Func<Hero, GenericData> _data;
            private TextObject txt;
            private Func<Hero, string> value;

            public StatObject(Func<Hero, GenericData> data, Func<Hero, string> value)
            {
                this.value=value;
                this.Height=32;

                _data=data;
                this.AddChild(new Icon(()=> data(Component).Icon.AsmImg(),()=>data(this.Component)));

                txt = this.AddText("".AsDrawText().Calibri().InColor(Global.CommonColorLight).InSize(28),36,5);
            }

            private class Icon : ImageControl, ITooltipedCustom
            {
                public bool ShowTooltip => true;
                private Func<GenericData> data;

                public Icon(Func<string> icon, Func<GenericData> data):base(icon)
                {
                    this.data=data;
                    this.Width=32;
                    this.Height=32;
                }

                public ISceneObject GetTooltip()
                {
                    return new GenericPanel(data());
                }
            }

            public override void Update(GameTimeLoop gameTime)
            {
                txt.SetText(value(Component));
                base.Update(gameTime);
            }
        }

        class PeaceAbility : ActiveHeroControl, ITooltiped, IMouseHint, ICursored
        {
            public string TooltipText => throw new NotImplementedException();

            public CursorImage Cursor => CursorImage.Hand;

            public ISceneObjectHosted CreateMouseHint()
            {
                throw new NotImplementedException();
            }
        }

        class SpecBadge : ActiveHeroControl, ITooltiped
        {
            public SpecBadge()
            {
                this.Width=60;
                this.Height=60;
            }

            public override string Image => $"Enums/Archetype/vanguard-{Component.Archetype.ToString().ToLowerInvariant()}.png".AsmImg();

            public string TooltipText => $"Бонусы специализации: нет";
        }

        class HeroName : ActiveHeroObject
        {
            TextObject text;
            public HeroName()
            {
                this.Width = 400;
                this.Height=35;
                text = this.AddTextCenter(Party.Active.Name.HeroName().InSize(30));
            }

            public override void Update(GameTimeLoop gameTime)
            {
                text.SetText(Component.Name);
                this.CenterChildText(text);
                base.Update(gameTime);
            }
        }

        class InfoAvatar : ActiveHeroControl, ITooltiped
        {
            public InfoAvatar()
            {
                this.Width=128;
                this.Height=128;
            }

            public string TooltipText => Component.Name;

            public override string Image => Component.Chip;

            public override Hero Component => Global.Game.Party.Active;
        }
    }

    internal class ActiveHeroControl : SceneControl<Hero>
    {
        public ActiveHeroControl() : base(null) { }

        public override Hero Component => Global.Game.Party.Active;
    }

    internal class ActiveHeroObject : SceneObject<Hero>
    {
        public ActiveHeroObject():base(null) { }

        public Party Party => Global.Game.Party;

        public override Hero Component => Global.Game.Party.Active;
    }
}
