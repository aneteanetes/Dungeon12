namespace Dungeon.Drawing.SceneObjects.Map
{
    using Dungeon.Control.Pointer;
    using Dungeon.Drawing.GUI;
    using Dungeon.Map.Objects;
    using Dungeon.View.Interfaces;
    using System;

    public class MoneySceneObject : TooltipClickableSceneObject<Money>
    {
        public override string Cursor => "takeloot";

        protected override string ClickableTooltipCursor => "takeloot";

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

            this.Image = $"Dungeon.Resources.Images.Items.gold_{amountImage}.png";
            this.ImageRegion = new Types.Rectangle(0, 0, 24, 24);

            this.Height = 0.5;
            this.Width = 0.5;

            this.Left = @object.Location.X;
            this.Top = @object.Location.Y;
        }

        protected override void Action(MouseButton mouseButton) => TakeMoney();

        private void TakeMoney()
        {
            playerSceneObject.Avatar.Character.Gold += @object.Amount;

            this.ShowEffects(new PopupString($"Вы нашли {@object.Amount} золота!", ConsoleColor.Yellow, new Types.Point(this.Left, this.Top), 25, 12, 0.06)
                .InList<ISceneObject>());

            this.Destroy?.Invoke();
            this.@object.Destroy?.Invoke();
        }

        protected override void OnTooltipClick() => TakeMoney();

        protected override void StopAction() { }
    }
}