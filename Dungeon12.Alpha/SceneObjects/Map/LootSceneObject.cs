namespace Dungeon12.Drawing.SceneObjects.Map
{
    using Dungeon.Control.Pointer;
    using Dungeon12.Items.Enums;
    using Dungeon12.Map.Objects;
    using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
    using Dungeon.View.Interfaces;
    using System.Collections.Generic;
    using Dungeon;

    public class LootSceneObject : TooltipClickableSceneObject<Loot>
    {
        public override string CursorOld => "takeloot";

        protected override string ClickableTooltipCursor => "takeloot";

        public LootSceneObject(PlayerSceneObject playerSceneObject, Loot @object, string tooltip) : base(playerSceneObject, @object, tooltip)
        {
            this.Image = @object.CustomLootImage ?? "Dungeon12.Resources.Images.Items.loot.png";

            if (@object.CustomLootImage == default)
            {
                this.ImageRegion = new Dungeon.Types.Rectangle(0, 0, 16, 16);
                this.Height = 0.5;
                this.Width = 0.5;
            }
            else
            {
                this.Height = 1;
                this.Width = 1;
            }

            this.Left = @object.Location.X;
            this.Top = @object.Location.Y;

            this.TooltipTextColor = @object.CustomLootColor ?? @object.Item.Rare.Color();
        }

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }

        protected override void Action(MouseButton mouseButton) => AddItemBackpack();

        private void AddItemBackpack()
        {
            this.@object.PickUp();
            if (this.@object.Item != default)
            {
                playerSceneObject.Avatar.Character.Backpack.Add(this.@object.Item, owner: playerSceneObject.Component.Entity);
                this.ShowInScene(new PopupString($"Вы нашли {@object.Item.Name}!", this.TooltipTextColor, new Dungeon.Types.Point(this.Left, this.Top), 25, 12, 0.06)
                    .InList<ISceneObject>());

                this.Destroy?.Invoke();
                this.@object.Destroy?.Invoke();
                this.@object.TakeTrigger?.Trigger<ITrigger<bool, string[]>>().Trigger(this.@object.TakeTriggerArguments);
            }
        }

        protected override bool CheckActionAvailable(MouseButton mouseButton)
        {
            var @base = base.CheckActionAvailable(mouseButton);
            if(!@base)
            {
                Global.GameState.Player.BindMovePointAction(this.@object, AddItemBackpack);
            }
            return @base;
        }

        protected override void OnTooltipClick() => AddItemBackpack();

        protected override void StopAction() { }

        private readonly Dictionary<Rarity, IDrawColor> RarityColors = new Dictionary<Rarity, IDrawColor>()
        {

        };
    }
}