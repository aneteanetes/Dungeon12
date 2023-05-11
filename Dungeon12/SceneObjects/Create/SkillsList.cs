using Dungeon;
using Dungeon.Control;
using Dungeon.Localization;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;
using Dungeon12.SceneObjects.Base;
using System.Linq;

namespace Dungeon12.SceneObjects.Create
{
    internal class SkillsList : SceneControl<Hero>
    {
        public SkillsList(Hero component) : base(component)
        {
            this.Height = 212;
            this.Width = 242;

            var title = this.AddTextCenter(Global.Strings["Skills"].AsDrawText().Gabriela().InColor(Global.CommonColor).InSize(35), vertical: false);
            title.Top = 5;

            var skillgroups = typeof(Skill).All<Skill>()
                .Select(x => new
                {
                    val = x,
                    g = x.ToValue<string>()
                })
                .GroupBy(x => x.g, x => x.val);

            foreach (var group in skillgroups)
            {
                var top = 65;
                foreach (var skill in group)
                {
                    this.AddChild(new SkillListItem(Component, skill)
                    {
                        Top = top
                    });

                    top += 40;
                }
            }
        }

        internal class SkillListItem : SceneControl<Hero>, ITooltipedCustom, IMouseHint
        {
            Skill _skill;

            public SkillListItem(Hero component, Skill skill) : base(component)
            {
                _skill = skill;
                this.Width = 242;
                this.Height = 35;

                this.AddTextCenter(skill.Display().AsDrawText().Gabriela().InColor(Global.CommonColor).InSize(25));
            }

            public override bool Visible => Component.Archetype == _skill.Class();

            public IDrawText TooltipText => $"{Global.Strings["RightMouseButton"]} - {Global.Strings["Info"]}".AsDrawText().Gabriela();

            public bool ShowTooltip => true;

            public ISceneObjectHosted CreateMouseHint() =>
                new ObjectPanel(_skill.Display(), Global.Strings.Description[_skill.ToString()]);

            public ISceneObject GetTooltip() => new GenericPanel(_skill.GenericData());

            public void RefreshTooltip() { }
        }
    }
}