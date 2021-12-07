using Dungeon;
using Dungeon.Control;
using Dungeon.SceneObjects;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.SceneObjects.UserInterface.CraftSelect
{
    public class CraftBadge : SceneControl<Hero>, ITooltipedCustom
    {
        private Crafts prof;

        public CraftBadge(Hero comp, Crafts craft) : base(comp)
        {
            prof = craft;

            this.Image = $"Professions/{craft}.png".AsmImg();

            var m = this.MeasureImage(this.Image);

            this.Width = m.X;
            this.Height = m.Y;

            SetPostion(craft);

            if (comp.Profession != null && comp.Profession == craft)
            {
                this.Focus();
                this.Click(null);
            }
                
        }

        private static CraftBadge selected;

        public override void Click(PointerArgs args)
        {
            if (selected == this)
                return;

            if (selected != default)
            {
                selected.Unselect();
                selected = null;
            }

            Component.Profession = prof;
            selected = this;
            Focus();
        }

        public override void Focus()
        {
            if (selected != this)
                Image = Image.Replace(".png", "_f.png");
        }

        public override void Unfocus()
        {
            if (selected != this)
                Image = Image.Replace("_f.png", ".png");
        }

        private void Unselect()
        {
            Image = Image.Replace("_f.png", ".png");
            Component.Profession = null;
        }

        private void SetPostion(Crafts craft)
        {
            switch (craft)
            {
                case Crafts.Alchemist:
                    Left = 95;
                    Top = 243;
                    break;
                case Crafts.Blacksmith:
                    Left = 235;
                    Top = 262;
                    break;
                case Crafts.Carpenter:
                    Left = 394;
                    Top = 249;
                    break;
                case Crafts.Tailor:
                    Left = 159;
                    Top = 423;
                    break;
                case Crafts.Artificer:
                    Left = 347;
                    Top = 422;
                    break;
                default:
                    break;
            }
        }

        public bool ShowTooltip => true;

        public Tooltip GetTooltip()
        {
            var title = prof.ToDisplay();
            var descr = prof.ToValue<string>();
            return new GameTooltip(title, descr, 400);
        }
    }
}