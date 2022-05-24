using Dungeon;
using Dungeon.Control;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using System;

namespace Dungeon12.SceneObjects.UI
{
    internal class PanelButton : EmptySceneControl, ITooltipedDrawText
    {
        string tooltip;

        public PanelButton(string img, string tooltip)
        {
            this.Width = 75;
            this.Height = 75;
            this.Image = img;
            this.tooltip = tooltip;
        }

        public Action OnClick { get; set; }

        public IDrawText TooltipText => tooltip.AsDrawText().Gabriela();

        public bool ShowTooltip => true;

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
            base.Click(args);
        }

        public void RefreshTooltip() { }
    }
}
