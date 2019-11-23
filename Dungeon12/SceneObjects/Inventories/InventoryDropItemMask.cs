namespace Dungeon12.Drawing.SceneObjects.Inventories
{
    using Force.DeepCloner;
    using Dungeon12.Drawing.SceneObjects.Main.CharacterInfo;
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Drawing.SceneObjects.UI;
    using Dungeon.Map;
    using Dungeon.Map.Objects;

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
                if (source.Parent is Inventory invContainer) //получить таргет
                {
                    if (invContainer.Parent is CharacterInfoWindow)
                    {
                        playerSceneObject.Avatar.Character.Backpack.Remove(source.Item,playerSceneObject.Component.Entity);
                        AddLootToMap(source);

                        inventory.Refresh();
                    }
                    else
                    {
                        source.Destroy?.Invoke();
                        invContainer.Refresh();
                    }
                }
            }

            base.OnDrop(source);
        }

        private void AddLootToMap(InventoryItem source)
        {
            var lootItem = new Loot()
            {
                Item = source.Item
            };

            lootItem.Location = gameMap.RandomizeLocation(playerSceneObject.Avatar.Location.DeepClone());
            lootItem.Destroy += () => gameMap.MapObject.Remove(lootItem);

            gameMap.MapObject.Add(lootItem);
            gameMap.PublishObject(lootItem);
        }
    }
}