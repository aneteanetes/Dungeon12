namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Events;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.GUI;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Drawing.SceneObjects.Main.CharacterInfo;
    using Rogue.Items.Enums;
    using Rogue.Loot;
    using Rogue.Map.Objects;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public class LootSceneObject : TooltipClickableSceneObject<Loot>
    {
        public override string Cursor => "takeloot";

        protected override string ClickableTooltipCursor => "takeloot";

        public LootSceneObject(PlayerSceneObject playerSceneObject, Loot @object, string tooltip) : base(playerSceneObject, @object, tooltip)
        {
            this.Image = "Rogue.Resources.Images.Items.loot.png";
            this.ImageRegion = new Types.Rectangle(0, 0, 16, 16);

            this.Height = 0.5;
            this.Width = 0.5;

            this.Left = @object.Location.X;
            this.Top = @object.Location.Y;

            this.TooltipTextColor = @object.Item.Rare.Color();
        }

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }

        protected override void Action(MouseButton mouseButton) => AddItemBackpack();

        private void AddItemBackpack()
        {
            playerSceneObject.Avatar.Character.Backpack.Add(this.@object.Item);
            this.ShowEffects(new PopupString($"Вы нашли {@object.Item.Name}!", this.TooltipTextColor, new Types.Point(this.Left, this.Top), 25, 12, 0.06)
                .InList<ISceneObject>());

            this.Destroy?.Invoke();
            this.@object.Destroy?.Invoke();
        }

        protected override void OnTooltipClick() => AddItemBackpack();

        protected override void StopAction() { }

        private readonly Dictionary<Rarity, IDrawColor> RarityColors = new Dictionary<Rarity, IDrawColor>()
        {

        };
    }
}