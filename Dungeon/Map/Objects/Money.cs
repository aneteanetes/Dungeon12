namespace Dungeon.Map.Objects
{
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Entities.Enemy;
    using Dungeon.Game;
    using Dungeon.Map.Infrastructure;
    using Dungeon.View.Interfaces;

    [Template("m")]
    public class Money : MapObject
    {
        public int Amount { get; set; }

        protected override MapObject Self => this;

        public override bool Interactable => true;

        public override ISceneObject View(GameState gameState)
        {
            return new MoneySceneObject(gameState.Player, this, $"Золото ({this.Amount})");
        }
    }
}
