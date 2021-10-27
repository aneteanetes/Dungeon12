namespace Dungeon12.Map.Objects
{
    using Dungeon12.Drawing.SceneObjects.Map;
    using Dungeon12.Game;
    using Dungeon12.Map.Infrastructure;
    using Dungeon.View.Interfaces;

    [Template("m")]
    public class Money : MapObject
    {
        public int Amount { get; set; }

        protected override MapObject Self => this;

        public override bool Interactable => true;

        public override ISceneObject Visual()
        {
            Global.AudioPlayer.Effect("golddrop.wav".AsmSoundRes());
            return new MoneySceneObject(Global.GameState.Player, this, $"Золото ({this.Amount})");
        }
    }
}
