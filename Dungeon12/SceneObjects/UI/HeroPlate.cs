using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.SceneObjects.UserInterface.Common;

namespace Dungeon12.SceneObjects.UI
{
    public class HeroPlate : SceneControl<Hero>
    {
        public HeroPlate(Hero component) : base(component)
        {
            if (component == null)
                Visible = false;

            this.Width = 400;
            this.Height = 150;

            this.Image = "Nameplate/background.png".AsmImg();
        }

        public void ReLoad(Hero component)
        {
            if (component == null)
                return;

            Visible = true;

            this.ClearChildrens();

            this.AddChild(new ImageObject($"SpecAvas/{component.Spec}.png".AsmImg())
            {
                Width = 85,
                Height = 130,
                Left = 15,
                Top = 10
            });

            this.AddChild(new ImageObject($"Nameplate/specback.png".AsmImg()));
            this.AddChild(new ImageObjectTooltiped($"Icons/Specs/{component.Spec}.png".AsmImg(),component.Spec.ToDisplay()));

            var name = this.AddTextCenter(component.Name.AsDrawText().Gabriela());
            name.Left = 116;
            name.Top = 12;

            this.AddChild(new Hp(component)
            {
                Left = 100,
                Top = 93
            });

            this.AddChild(new ImageObjectTooltiped($"Nameplate/lvlback.png".AsmImg(), "Уровень персонажа")
            {
                Left = 80,
                Top = 120
            });

            var txt = this.AddTextCenter(component.Level.ToString().AsDrawText().InSize(15).Gabriela());
            txt.Left = 89;
            txt.Top = 122;
        }

        private class Hp : SceneControl<Hero>, ITooltiped
        {
            TextControl textPercent;

            public Hp(Hero component) : base(component)
            {
                this.Image = $"Nameplate/hp.png".AsmImg();
                imageRegion = new Rectangle()
                {
                    Width = 300,
                    Height = 50,
                    X = 0,
                    Y = 0
                };

                textPercent = this.AddTextCenter($"{Component.Hits / Component.MaxHits * 100}%".AsDrawText().Gabriela().InSize(15), false, false);
                textPercent.Left = 5;
                textPercent.Top -= 23;
            }

            public override void Focus()
            {
                base.Focus();
            }

            public override void Update(GameTimeLoop gameTime)
            {
                textPercent.Text.SetText($"{Component.Hits / Component.MaxHits * 100}%");
                base.Update(gameTime);
            }

            public void RefreshTooltip()
            {
                TooltipText = $"{Component.Hits}/{Component.MaxHits}".AsDrawText().Gabriela().InSize(12);
            }

            private Rectangle imageRegion;
            public override Rectangle ImageRegion
            {
                get
                {
                    var percent = Component.Hits / Component.MaxHits;
                    imageRegion.Width *= percent;
                    imageRegion.X *= percent;
                    return imageRegion;
                }
            }

            public IDrawText TooltipText { get; set; }

            public bool ShowTooltip => true;
        }
    }
}
