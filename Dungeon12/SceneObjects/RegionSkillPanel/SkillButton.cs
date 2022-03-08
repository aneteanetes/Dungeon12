using Dungeon;
using Dungeon.Control;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Enums;
using System;

namespace Dungeon12.SceneObjects.RegionSkillPanel
{
    public class SkillButton : EmptySceneControl, ITooltiped, ICursored
    {
        public SkillButton(Skill skill, Action click)
        {
            Width = 75;
            Height = 75;
            Image = $"UI/layout/{skill}.png".AsmImg();

            OnClick = click;

            TooltipText = Global.Strings.ByProperty(skill.ToString()).AsDrawText().Gabriela();
        }

        public Action OnClick { get; set; }

        public IDrawText TooltipText { get; set; }

        public bool ShowTooltip => true;

        public CursorImage Cursor => CursorImage.Info;

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
            base.Click(args);
        }
    }
}
