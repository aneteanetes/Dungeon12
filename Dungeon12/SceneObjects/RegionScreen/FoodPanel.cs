﻿using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Nabunassar.ECS.Components;
using Nabunassar.Entities;

namespace Nabunassar.SceneObjects.RegionScreen
{
    internal class FoodPanel : SceneControl<Party>
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public FoodPanel(Party component) : base(component)
        {
            this.Height = 65;
            this.Width = 350;

            var left = 300;

            foreach (var food in component.Food.Components)
            {
                this.AddChild(new FoodPlate(food)
                {
                    Left = left
                });
                left -= 60;
            }
        }

        private class FoodPlate : SceneControl<Food>, ITooltipedDrawText
        {
            public override void Throw(Exception ex)
            {
                throw ex;
            }

            public FoodPlate(Food component) : base(component)
            {
                this.Width = 50;
                this.Height = 65;
                this.Image = "UI/layout/foodframe.png".AsmImg();
                this.AddChild(new ImageObject(() => Component.Image)
                {
                    Width = 45,
                    Height = 45,
                    Top = 10,
                    Left = 2
                });

                TooltipText=$"{component.Name}: {component.Value} ({component.Quality}%)"
                    .AsDrawText()
                    .Gabriela();
            }

            public override bool Visible => Component.Value > 0;

            public IDrawText TooltipText { get; set; }

            public bool ShowTooltip => true;

            public override void Update(GameTimeLoop gameTime)
            {
                TooltipText.SetText($"{Component.Name}: {Component.Value} ({Component.Quality}%)");
                base.Update(gameTime);
            }
        }
    }
}