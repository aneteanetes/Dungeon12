using Dungeon;
using Dungeon.Control;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using System;

namespace Dungeon12.SceneObjects.Create
{
    public class ArrowBtn : EmptySceneControl, ITooltiped
    {
        private bool _right;

        public ArrowBtn(bool right=true)
        {
            _right = right;
            this.Width = 70;
            this.Height = 70;
            this.Image = $"UI/start/{(right ? "next" : "prev")}.png".AsmImg();
        }

        public Action OnClick { get; set; }

        public IDrawText TooltipText => $"{(_right ? Global.Strings.Next : Global.Strings.Cancel)}".AsDrawText().Gabriela();

        public bool ShowTooltip => true;

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
            base.Click(args);
        }

        public void RefreshTooltip() { }
    }
}