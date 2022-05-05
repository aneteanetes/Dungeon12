using Dungeon;
using Dungeon.Drawing;
using Dungeon.Resources;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;
using Dungeon12.SceneObjects.UserInterface.Common;
using Dungeon12.SceneObjects.UserInterface.FractionSelect;

namespace Dungeon12.SceneObjects.UserInterface.SpecSelect
{
    public class SpecCard : EmptySceneControl
    {
        SpecInfo SpecInfo;

        public static double WidthCard = 375;
        public static double HeightCard = 640;
        public static double CardSpace = 25;

        public SpecCard(Spec spec)
        {
            SpecInfo = ResourceLoader.LoadJson<SpecInfo>($"Skills/{spec}.json".AsmRes());

            this.Width = WidthCard;
            this.Height = HeightCard;

            this.Image = $"SpecCard/{spec}.png".AsmImg();

            var titletxt = Runed(spec.ToDisplay()).InSize(40);
            var title = this.AddTextCenter(titletxt, vertical: false);
            title.Top = 52;

            this.AddChild(new ImageObjectTooltiped("Icons/Roles/Health.png".AsmImg(), "Выживаемость")
            {
                Width = 34,
                Height = 34,
                Left = 59,
                Top = 362
            });

            var htxt = this.AddTextCenter(Runed($"{SpecInfo.Health}/3").InSize(20));
            htxt.Top = 362;
            htxt.Left = 97;

            this.AddChild(new ImageObjectTooltiped($"Icons/Roles/{SpecInfo.DamageIcon}.png".AsmImg(), "Сила специализации")
            {
                Width = 34,
                Height = 34,
                Left = 153,
                Top = 362
            });

            var dpstxt = this.AddTextCenter(Runed($"{SpecInfo.Damage}/3").InSize(20));
            dpstxt.Top = 362;
            dpstxt.Left = 185;

            this.AddChild(new ImageObjectTooltiped("Icons/Roles/Armor.png".AsmImg(), "Уровень защиты")
            {
                Width = 34,
                Height = 34,
                Left = 242,
                Top = 362
            });

            var armtxt = this.AddTextCenter(Runed($"{SpecInfo.Armor}/3").InSize(20));
            armtxt.Top = 362;
            armtxt.Left = 277;

            var description = this.AddTextCenter(Runed(SpecInfo.Description).Gabriela().InSize(15).WithWordWrap(), parentWidth: 200);
            description.Top = 404;
            description.Left = 116;

            if (SpecInfo.IsEnabled)
            {
                var btn = new MapButton();
                btn.Width = 157;
                btn.Height = 40;
                btn.OnClick = () =>
                {
                    Global.Game.Party.Hero1.Spec = spec;
                    Global.Game.Party.Hero1.MaxHits = Global.Game.Party.Hero1.Hits = SpecInfo.Health * 50;
                    Global.Game.Party.Hero1.Chip = $"SpecChips/{spec}.png".AsmImg();

                    SetAssistanceSpec(spec);

                    SpecSelectSceneObject.Close();
                };

                this.AddChildCenter(btn);
                btn.Top = 560;
            }

            var topInit = 400;
            var iconRange = 40;

            SpecInfo.Skills.ForEach((skill, i) =>
            {
                var icon = this.AddChild(new SpecCardSkill(skill, i, spec));
                icon.Top = topInit + iconRange * i;
                icon.Left = 72;
            });


            var tcg = this.AddChild(new SpecCardSkill(SpecInfo.CardName));
            tcg.Top = 535;
            tcg.Left = 278;
        }

        private static void SetAssistanceSpec(Spec spec)
        {
            string image = "";
            switch (spec)
            {
                case Spec.WarriorWarchief:
                    image = Spec.PaladinAdept.ToString();
                    break;
                case Spec.MageAoe:
                    image = Spec.MercenaryLeader.ToString();
                    break;
                case Spec.MercenaryLeader:
                    image = Spec.MageAoe.ToString();
                    break;
                case Spec.PaladinAdept:
                    image = Spec.WarriorWarchief.ToString();
                    break;
                case Spec.WarlockNecromancer:
                    image = Spec.WarriorWarchief.ToString();
                    break;
                default:
                    break;
            }
            Global.Game.Location.Links[0].Polygon.ObjectImage = image + ".png";                        
        }

        private IDrawText Runed(string text)
        {
            return text.AsDrawText().Gabriela().InColor(System.ConsoleColor.Black);
        }
    }
}
