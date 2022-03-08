using Dungeon;
using Dungeon.Control;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using System;

namespace Dungeon12.SceneObjects.RegionScreen
{
    public class ControlButton : EmptySceneControl, ITooltiped
    {
        public ControlButton(string img, string tooltipText, Action click)
        {
            Height = 77;
            Width = 77;
            OnClick = click;
            Image = img.AsmImg();
            TooltipText = tooltipText.AsDrawText().Gabriela();
        }

        private readonly Action OnClick;

        public IDrawText TooltipText { get; set; }

        public bool ShowTooltip => true;

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
            base.Click(args);
        }
    }
}
