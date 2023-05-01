using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.Entities.Abilities;
using Dungeon12.Entities.Enums;
using Dungeon12.Entities.Talks;

namespace Dungeon12.SceneObjects.MUD
{
    internal class ResourcePanel : SceneControl<Party>
    {
        public ResourcePanel(Party component) : base(component)
        {
            this.Width=400;
            this.Height=30;

            this.AddBorder();
            
            var gold = this.AddChildCenter(new GoldSceneObject(component));
            gold.Left = 5;

            var good = this.AddChildCenter(new FameSceneObj(component.Fame, FameType.Good)
            {
                Left=105
            }, false);

            var bad = this.AddChildCenter(new FameSceneObj(component.Fame, FameType.Evil)
            {
                Left=155
            }, false);

            var tricks = this.AddChildCenter(new FameSceneObj(component.Fame, FameType.Trick)
            {
                Left=205
            }, false);

            var wisd = this.AddChildCenter(new FameSceneObj(component.Fame, FameType.Wisdom)
            {
                Left=255
            }, false);

            var prayers = this.AddChildCenter(new PrayersSceneObject(component));
            prayers.Left = this.Width-prayers.Width-10;
        }

        protected override ControlEventType[] Handles => new ControlEventType[0];

        private class GoldSceneObject : SceneControl<Party>, ITooltiped
        {
            private TextObject valueObj;

            public GoldSceneObject(Party comp):base(comp)
            {
                this.Height = 20;
                this.AddChild(new ImageObject("Icons/MUD/gold.tga") { Width=20, Height=20 });

                var txt = comp.Gold.ToString().AsDrawText().InBold().InSize(18).Calibri();
                valueObj = this.AddTextCenter(txt, false);
                valueObj.Left = 22;

                var txtMeasure = this.MeasureText(txt).X;
                this.Width=20+txtMeasure;
            }

            public override void Update(GameTimeLoop gameTime)
            {
                valueObj.SetText(Component.Gold.ToString());
                base.Update(gameTime);
            }

            public string TooltipText => Global.Strings["Gold"];
        }

        private class FameSceneObj : SceneControl<Fame>, ITooltiped
        {
            private TextObject txt;
            private FameType type;

            public FameSceneObj(Fame fame, FameType type) : base(fame)
            {
                this.type=type;

                this.Width = 35;
                this.Height = 20;

                this.AddChild(new ImageObject($"Icons/MUD/fame.{type.ToString().ToLowerInvariant()}.tga")
                {
                    Height=20,
                    Width=20,
                });

                txt = this.AddTextCenter(CommonDrawText(fame[type].ToString()), false);
                txt.Left=23.55;
            }

            public string TooltipText => Global.Strings["Glory"] +": "+Global.Strings[$"Fame.{type}"];

            public override void Update(GameTimeLoop gameTime)
            {
                txt.SetText(Component[type].ToString());
                base.Update(gameTime);
            }
        }

        private class PrayersSceneObject : SceneControl<Party>, ITooltiped
        {
            private TextObject lvl;
            private TextObject val;

            public PrayersSceneObject(Party heroes):base(heroes)
            {
                this.Height = 20;

                var prayer = heroes.Prayers.Current;

                var txtLvl = CommonDrawText(prayer.Level.ToString());
                var txtLvlWidth = this.MeasureText(txtLvl).X;

                var txtVal = CommonDrawText(prayer.Value.ToString());
                var txtValWidth = this.MeasureText(txtVal).X;

                lvl = this.AddTextCenter(txtLvl, false);

                var img = this.AddChild(new ImageObject("Icons/MUD/prayers.tga") { Width=20, Height=20 });
                img.Left = txtLvlWidth+5;

                val = this.AddTextCenter(txtVal, false);
                val.Left = 35;

                this.Width = txtLvlWidth+ 5+img.Width+5+txtValWidth;
            }

            public override void Update(GameTimeLoop gameTime)
            {
                lvl.SetText(Component.Prayers.Level.ToString());
                val.SetText(Component.Prayers.Value.ToString());
                base.Update(gameTime);
            }

            public string TooltipText => Global.Strings["Prayers"]+$": {Component.Prayers.Current.God.AsShimmer()}";
        }

        private static IDrawText CommonDrawText(string value) => value.ToString().AsDrawText().InBold().InSize(18).Calibri();
    }
}