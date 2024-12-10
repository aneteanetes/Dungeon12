﻿using Dungeon;
using Dungeon.Control;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Nabunassar.ECS.Components;
using System;

namespace Nabunassar.SceneObjects.Create
{
    internal class ArrowBtn : EmptySceneControl, ITooltipedDrawText
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        private bool _right;

        public ArrowBtn(bool right=true)
        {
            _right = right;
            this.Width = 70;
            this.Height = 70;
            this.Image = $"UI/start/{(right ? "next" : "prev")}.png".AsmImg();
        }

        public override string Image { get => base.Image; set => base.Image=value; }

        public Action OnClick { get; set; }

        public IDrawText TooltipText => $"{(_right ? Global.Strings["Next"] : Global.Strings["Cancel"])}".AsDrawText().Gabriela();

        public bool ShowTooltip => true;

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
            base.Click(args);
        }

        public void RefreshTooltip() { }

        public override void Destroy()
        {
            base.Destroy();
        }
    }
}