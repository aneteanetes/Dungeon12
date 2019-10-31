namespace Dungeon12.Map.Objects
{
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Game;
    using Dungeon.Map;
    using Dungeon.Map.Infrastructure;
    using Dungeon.Map.Objects;
    using Dungeon.View.Interfaces;

    [Template("H")]
    public class Home : Сonversational
    {
        public override string Icon { get => "N"; set { } }

        protected override MapObject Self => this;

        public override bool Obstruction => true;

        public override ISceneObject View(GameState gameState)
        {
            return new HomeSceneObject(gameState.Player, this, this.Name, gameState.Map);
        }
    }
}