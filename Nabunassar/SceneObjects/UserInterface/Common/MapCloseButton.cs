﻿using Dungeon;
using Dungeon.Control;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Nabunassar.ECS.Components;
using Nabunassar.SceneObjects.Base;
using System;

namespace Nabunassar.SceneObjects.UserInterface.Common
{
    internal class MapCloseButton : EmptySceneControl, ITooltipedDrawText
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public MapCloseButton()
        {
            this.Image = "Backgrounds/mapclose.png".AsmImg();
            Width = 58;
            Height = 100;
        }

        private bool _disabled;
        public bool Disabled
        {
            get => _disabled;
            set
            {
                _disabled = value;
            }
        }

        public Action OnClick { get; set; }

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
        }

        public override string Image { get; set; } = "Backgrounds/mapclose.png".AsmImg();

        public IDrawText TooltipText => "Закрыть".AsDrawText();

        public bool ShowTooltip => true;

        public Tooltip CustomTooltipObject => null;

        public override void Focus()
        {
            if (Disabled)
                return;

            Image = "Backgrounds/mapclose_f.png".AsmImg();
        }

        public override void Unfocus()
        {
            if (Disabled)
                return;

            Image = "Backgrounds/mapclose.png".AsmImg();
        }

        public void RefreshTooltip() { }
    }
}