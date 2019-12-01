namespace Dungeon.Map.Objects
{
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Game;
    using Dungeon.Items;
    using Dungeon.Map.Infrastructure;
    using Dungeon.View.Interfaces;

    [Template("l")]
    public class Loot : MapObject
    {
        public Item Item { get; set; }

        protected override MapObject Self => this;

        public override bool Interactable => true;

        public override ISceneObject Visual(GameState gameState)
        {
            return new LootSceneObject(gameState.Player, this, this.Item.Name);
        }
    }
}
