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

        public virtual string CustomLootImage { get; set; }

        public virtual IDrawColor CustomLootColor { get; set; }

        protected override MapObject Self => this;

        public override bool Interactable => true;

        public virtual string TakeTrigger { get; set; }

        public virtual string[] TakeTriggerArguments { get; set; }

        public override ISceneObject Visual(GameState gameState)
        {
            return new LootSceneObject(gameState.Player, this, this.Item.Name);
        }
    }
}
