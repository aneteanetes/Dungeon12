using Dungeon;
using Dungeon.Control;
using Dungeon.SceneObjects;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;

namespace Dungeon12.SceneObjects.UserInterface.FractionSelect
{
    public class FractionBadge : SceneControl<Hero>
    {
        private Fraction fraction;
        FractionSelectSceneObject mainobj;
        public FractionBadge(Hero comp, Fraction fract, FractionSelectSceneObject mainobj) : base(comp)
        {
            this.mainobj = mainobj;
            fraction = fract;

            PerPixelCollision = true;

            Image = $"Fractions/{fraction}.png".AsmImg();

            var m = MeasureImage(Image);

            Width = m.X;
            Height = m.Y;

            SetPostion();
        }

        private static FractionBadge selected;

        public override void Click(PointerArgs args)
        {
            if (selected == this)
                return;

            if (selected != default)
            {
                selected.Unselect();
                selected = null;
            }

            Component.Fraction = fraction;
            selected = this;
            Focus();

            mainobj.desc.Load(fraction);
        }

        private static FractionDescription desc;

        public override void Focus()
        {
            if (selected != this)
            {
                Image = Image.Replace(".png", "_f.png");
                mainobj.desc.Load(fraction);
            }
        }

        public override void Unfocus()
        {
            if (selected != this)
            {
                Image = Image.Replace("_f.png", ".png");
            }

            if (selected != null)
                mainobj.desc.Load(selected.fraction);
        }

        private void Unselect()
        {
            Image = Image.Replace("_f.png", ".png");
            Component.Profession = null;
        }

        private void SetPostion()
        {
            switch (fraction)
            {
                case Fraction.Vanguard:
                    Left = 312;
                    Top = -16;
                    break;
                case Fraction.MageGuild:
                    Left = 11;
                    Top = 11;
                    break;
                case Fraction.Mercenary:
                    Left = 115;
                    Top = 439;
                    break;
                case Fraction.Exarch:
                    Left = 173;
                    Top = 231;
                    break;
                case Fraction.Cult:
                    Left = 602;
                    Top = 3;
                    break;
                default:
                    break;
            }
        }
    }
}