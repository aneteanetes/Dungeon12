using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.Entities.Zones;
using Dungeon12.SceneObjects.UserInterface.Common;
using System;
using System.Linq;

namespace Dungeon12.SceneObjects.UserInterface.OriginSelect
{
    public class AreaDescription : EmptySceneControl
    {
        private EmptySceneObject Title1;
        private EmptySceneObject Title2;

        private EmptySceneObject Description;

        private TextControl bonustext;

        private MapButton button;

        ZonePerk perk;

        OriginSelectSceneObject originSelectSceneObject;

        public AreaDescription(OriginSelectSceneObject originSelectSceneObject)
        {
            this.originSelectSceneObject = originSelectSceneObject;

            this.Image = "Maps/desc.png".AsmImg();

            bonustext = this.AddTextCenter("Бонус: ".AsDrawText().Gabriela().InSize(20));
            bonustext.Left = 57;
            bonustext.Top = 305;

            button = this.AddChild(new MapButton()
            {
                Left = 13,
                Top = 357,
                OnClick = SelectZone
            });

            perk = this.AddChild(new ZonePerk()
            {
                Left = 145,
                Top = 301
            });

            var lineimg = this.AddChild(new ImageObject("Backgrounds/line230.png".AsmImg()));
            lineimg.Left = 25;
            lineimg.Top = 85;
        }

        private void SelectZone()
        {
            Global.Game.Party.Hero1.Perks.Add(@fixed.Perk);
            OriginSelectSceneObject.Selected = @fixed;
            originSelectSceneObject.Destroy?.Invoke();
        }

        Zone zone;

        bool inited;

        public void Reset()
        {
            if (@fixed == default)
                this.Visible = false;
        }

        private Zone @fixed;

        public void FixOn(Zone component)
        {
            @fixed = null;
            Refresh(component);
            @fixed = component;
        }

        public void FixOff(Zone component)
        {
            @fixed = null;
        }

        public void Refresh(Zone component)
        {
            if (@fixed != default)
                return;

            this.Visible = true;

            zone = component;
            perk.Bind(zone.Perk);

            var split = component.Name.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var titletext1 = split[0].AsDrawText().Gabriela().InSize(23);
            var titletext2 = split[1].AsDrawText().Gabriela().InSize(23);

            var desctext = component.Description
                .AsDrawText()
                .Gabriela()
                .InSize(12)
                .WithWordWrap();

            if (!inited)
            {
                Title1 = new EmptySceneObject()
                {
                    Width = 243,
                    Height = 34,
                    Left = 11,
                    Top = 13
                };
                Title1.AddTextCenter(titletext1);
                this.AddChild(Title1);

                Title2 = new EmptySceneObject()
                {
                    Width = 243,
                    Height = 34,
                    Left = 11,
                    Top = 43
                };
                Title2.AddTextCenter(titletext2);
                this.AddChild(Title2);

                Description = new EmptySceneObject()
                {
                    Width = 242,
                    Height = 213,
                    Left = 11,
                    Top = 94
                };
                Description.AddTextCenter(desctext,vertical:false);
                this.AddChild(Description);

                inited = true;
            }
            else
            {
                var txt1 = Title1.Children.First().As<TextControl>();
                var txt2 = Title2.Children.First().As<TextControl>();

                txt1.SetText(titletext1);
                txt2.SetText(titletext2);

                Title1.CenterText(txt1);
                Title2.CenterText(txt2);

                Description.Children.First().As<TextControl>().SetText(desctext);
            }

            button.Visible = component.Selectable;
            bonustext.Visible = component.Selectable;
            perk.Visible = component.Selectable;
        }
    }
}
