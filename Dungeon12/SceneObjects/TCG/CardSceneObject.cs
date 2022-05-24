using Dungeon;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.SceneObjects.UserInterface.Common;
using Dungeon12.TCG;

namespace Dungeon12.SceneObjects.TCG
{
    internal class CardSceneObject : SceneControl<Card>
    {
        public CardSceneObject(Card component) : base(component, false)
        {
            this.Width = 500;
            this.Height = 700;

            this.AddChild(new CardBackground(component));

            this.AddChild(new ImageObjectTooltiped($"TCG/kit/types/{component.Type.ToString().ToLowerInvariant()}.png".AsmImg(), component.Type.ToDisplay()));

            if (component.Subtype != null)
                this.AddChild(new ImageObjectTooltiped($"TCG/kit/subtypes/{component.Subtype.ToString().ToLowerInvariant()}.png".AsmImg(), component.Subtype.ToDisplay()));

            if (component.Cost != null)
            {
                this.AddChild(new ImageObjectTooltiped($"TCG/kit/cost.png".AsmImg(), "Стоимость"));
                var costtxt = this.AddTextCenter(component.Cost.ToString().AsDrawText().Gabriela().InSize(30));
                costtxt.Left = 45;
                costtxt.Top = 200;
            }

            if (component.Power != null)
            {
                this.AddChild(new ImageObjectTooltiped($"TCG/kit/power.png".AsmImg(), "Сила"));
                var powertxt = this.AddTextCenter(component.Cost.ToString().AsDrawText().Gabriela().InSize(30));
                powertxt.Left = 30;
                powertxt.Top = 314;
            }

            var description = this.AddTextCenter(component.Description.AsDrawText().Gabriela().InColor(new DrawColor(31,21,16)).InSize(20).WithWordWrap(), true, false, 380);
            description.Left = 68;
            description.Top = 535;
        }

        private class CardBackground : SceneObject<Card>
        {

            public CardBackground(Card component) : base(component)
            {
                this.Width = 500;
                this.Height = 700;

                this.AddChild(new ImageObject("TCG/kit/card.png".AsmImg()));
                this.AddChild(new ImageObject($"TCG/arts/{component.Image}".AsmImg())
                {
                    Width = 325,
                    Height = 325,
                    Left = 85,
                    Top = 85
                });
                this.AddChild(new ImageObject("TCG/kit/design.png".AsmImg()));
                this.AddChild(new ImageObject($"TCG/kit/banner/{((int)component.Region)}.png".AsmImg()));

                var nametxt = this.AddTextCenter(component.Name.AsDrawText().FrizQuad().InSize(27).InBold(), true, false, 366);
                nametxt.Left = 47;
                nametxt.Top = 35;

                //this.AddChild(new ImageObject($"TCG/kit/banner/overlay.png".AsmImg()));
            }
        }
    }
}
