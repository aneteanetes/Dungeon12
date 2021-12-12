using Dungeon;
using Dungeon.SceneObjects;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.SceneObjects.UserInterface.FractionSelect
{
    public class SpecCardSkill : EmptySceneControl, ITooltipedCustom
    {
        public bool ShowTooltip => skill.Name != null;

        private string title;
        private string description;

        SkillInfo skill;
        public SpecCardSkill(SkillInfo skill,int i, Spec spec)
        {
            Width = 33;
            Height = 33;

            this.skill = skill;
            if (skill.Name != default)
            {
                title = skill.Name;
                description = skill.Description.Replace(".", "\r\n\r\n");
                this.Image = $"Icons/Skills/{spec}{i}.png".AsmImg();
            }
            else
            {
                this.Image = $"Icons/Skills/empty.png".AsmImg();
            }
        }

        public SpecCardSkill(string cardName)
        {
            Width = 33;
            Height = 33;

            skill = new SkillInfo() { Name = cardName };
            title = cardName;

            this.Image = $"Icons/Skills/tcgcards.png".AsmImg();
        }

        public Tooltip GetTooltip()
        {
            return new GameTooltip(title, description, description == null ? 0 : 325, description == null);
        }
    }
}