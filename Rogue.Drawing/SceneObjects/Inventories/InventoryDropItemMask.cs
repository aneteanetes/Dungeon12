namespace Rogue.Drawing.SceneObjects.Inventories
{
    using Force.DeepCloner;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Map;

    public class InventoryDropItemMask : DropableControl<InventoryItem>
    {
        public override bool AbsolutePosition => true;
        public override bool CacheAvailable => true;

        private PlayerSceneObject playerSceneObject;
        private Inventory inventory;
        private GameMap gameMap;

        public InventoryDropItemMask(PlayerSceneObject playerSceneObject, Inventory inventory, GameMap gameMap)
        {
            this.gameMap = gameMap;
            this.inventory = inventory;
            this.playerSceneObject = playerSceneObject;
            this.Width = 40;
            this.Height = 22.5;
        }

        protected override void OnDrop(InventoryItem source)
        {
            if (source.DropProcessed == 1)
            {
                playerSceneObject.Avatar.Character.Backpack.Remove(source.Item);
                inventory.Refresh();
                AddLootToMap(source);
            }

            base.OnDrop(source);
        }

        private void AddLootToMap(InventoryItem source)
        {
            var lootItem = new Rogue.Map.Objects.Loot()
            {
                Item = source.Item
            };

            lootItem.Location = gameMap.RandomizeLocation(playerSceneObject.Avatar.Location.DeepClone());
            lootItem.Destroy += () => gameMap.Map.Remove(lootItem);

            gameMap.Map.Add(lootItem);
            gameMap.PublishObject(lootItem);
        }
    }
}