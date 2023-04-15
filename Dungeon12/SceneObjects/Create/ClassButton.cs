﻿using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;

namespace Dungeon12.SceneObjects.Create
{
    internal class ClassButton : SceneControl<Hero>, ITooltipedDrawText
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        Archetype _archetype;

        Selector _selector;

        public ClassButton(Hero component, Archetype archetype) : base(component)
        {
            _archetype = archetype;
            this.Width = 55;
            this.Height = 55;

            this.Image = $"Enums/Archetype/{archetype}.png".AsmImg();
            _selector = this.AddChild(new Selector(Component,archetype));
        }

        private class Selector : SceneControl<Hero>
        {
            public override void Throw(Exception ex)
            {
                throw ex;
            }

            Archetype _archetype;

            public Selector(Hero c, Archetype archetype) : base(c)
            {
                _archetype = archetype;
                   Image = "UI/start/classselector.png".AsmImg();
                Width = 60;
                Height = 60;
                Left = -2.5;
                Top = -2.5;
            }

            public override bool Visible => Component.Archetype == _archetype;
        }

        public override void Click(PointerArgs args)
        {
            Component.Archetype = _archetype;
            base.Click(args);
        }

        public IDrawText TooltipText => _archetype.Display().AsDrawText().Gabriela();

        public bool ShowTooltip => true;

        public void RefreshTooltip() { }
    }
}