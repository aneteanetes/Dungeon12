namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Pointer;
    using Rogue.Drawing.GUI;
    using Rogue.Map.Objects;
    using Rogue.View.Interfaces;
    using System;

    public class MoneySceneObject : ClickActionSceneObject<Money>
    {
        public MoneySceneObject(PlayerSceneObject playerSceneObject, Money @object, string tooltip) : base(playerSceneObject, @object, tooltip)
        {
            var amountImage = "";

            if (@object.Amount < 100)
            {
                amountImage = "s";
            }
            if (@object.Amount > 300 && @object.Amount < 500)
            {
                amountImage = "n";
            }
            if (@object.Amount > 500 && @object.Amount < 1000)
            {
                amountImage = "l";
            }
            if (@object.Amount > 1000)
            {
                amountImage = "b";
            }

            this.Image = $"Rogue.Resources.Images.Items.gold_{amountImage}.png";
            this.ImageRegion = new Types.Rectangle(0, 0, 24, 24);

            this.Height = 0.75;
            this.Width = 0.75;

            this.Left = @object.Location.X;
            this.Top = @object.Location.Y;
        }

        protected override void Action(MouseButton mouseButton)
        {
            playerSceneObject.Avatar.Character.Gold += @object.Amount;

            this.ShowEffects(new PopupString($"Вы нашли {@object.Amount} золота!", ConsoleColor.Yellow, new Types.Point(this.Left, this.Top), 25, 19, 0.06)
                .InList<ISceneObject>());

            this.Destroy?.Invoke();
            this.@object.Destroy?.Invoke();
        }

        protected override void StopAction() { }
    }
}