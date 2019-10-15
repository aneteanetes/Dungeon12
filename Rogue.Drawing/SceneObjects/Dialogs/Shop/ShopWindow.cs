namespace Rogue.Drawing.SceneObjects.Dialogs.Shop
{
    using Rogue.Control.Keys;
    using Rogue.Drawing.SceneObjects.Common;
    using Rogue.Drawing.SceneObjects.Inventories;
    using Rogue.Drawing.SceneObjects.Main.CharacterInfo;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Map;
    using Rogue.View.Interfaces;
    using System;

    public class ShopWindow : DraggableControl<ShopWindow>
    {
        public override int Layer => 50;

        public override bool AbsolutePosition => true;

        protected override Key[] OverrideKeyHandles => new Key[] { Key.Escape };

        public ShopWindow(string title, PlayerSceneObject playerSceneObject, Merchants.Merchant shop, Action<ISceneObject> destroyBinding, Action<ISceneObjectControl> controlBinding, GameMap gameMap)
        {
            Global.FreezeWorld = this;

            this.Top = 2;
            this.Left = 0;

            var charInfo = new CharacterInfoWindow(gameMap, playerSceneObject, this.ShowEffects, false,false)
            {
                Left = 16 + 5,
                DisableDrag = true
            };
            charInfo.Top = 0;

            var shopWindow = new ShopWindowContent(title, shop, playerSceneObject, charInfo.Inventory)
            {
                Left = 5,
            };

            shopWindow.BindCharacterInventory(charInfo.Inventory);

            this.Width = 28;
            this.Height = 17;

            this.AddChild(shopWindow);
            this.AddChild(charInfo);
            this.AddChild(new InventoryDropItemMask(playerSceneObject, charInfo.Inventory, gameMap)
            {
                Top = -2
            });
        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold) => Close();

        private void Close()
        {
            this.Destroy?.Invoke();
            Global.FreezeWorld = null;
            SkillControl.RestoreClick();
        }
    }
}