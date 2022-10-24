using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.Entities;

namespace Dungeon12.SceneObjects.UI
{
    internal class FoodCounter : SceneControl<Food>
    {
        public FoodCounter(Food component) : base(component, true)
        {
            this.Width = 255;
            this.Height = 100;
            Image = "UI/food.png".AsmImg();
            this.AddChild(tooltip = new FoodTooltip(component)
            {
                Visible = false
            });

            var count = this.AddChild(new Counter(Component));
            count.Left = 100;
            count.Top = 20;
        }

        private class Counter : SceneObject<Food>
        {
            public Counter(Food food) : base(food)
            {
            }

            public override IDrawText Text => $"{Component.Value}/{Component.Max}".AsDrawText().Gabriela().InSize(34);
        }


        private FoodTooltip tooltip;

        public override void Focus()
        {
            tooltip.SetPosition(new Dot(50, 100));
            tooltip.Visible = true;

            base.Focus();
        }

        public override void Unfocus()
        {
            tooltip.Visible = false;
            base.Unfocus();
        }

        private class FoodTooltip : DarkRectangle
        {
            public override bool Filtered => false;

            public override bool Interface => true;

            public IDrawText TooltipText => Text;

            Food food;

            public FoodTooltip(Food food)
            {
                this.food = food;
                Opacity = 0.8;

                var textSize = MeasureText($"Эффективность: {food.Quality}%".AsDrawText().Gabriela());

                Width = textSize.X + 10;
                Height = textSize.Y + 5;
            }

            public override IDrawText Text => $"Эффективность: {food.Quality}%".AsDrawText().Gabriela();

            public void SetPosition(Dot position)
            {
                Left = position.X;
                Top = position.Y;
            }
        }
    }
}