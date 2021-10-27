namespace Dungeon12.Map.Objects
{
    using Dungeon12.Drawing.SceneObjects.Map;
    using Dungeon12.Game;
    using Dungeon12.Items;
    using Dungeon12.Map.Infrastructure;
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

        public virtual void PickUp() { }

        public override ISceneObject Visual()
        {
            Global.AudioPlayer.Effect("itemdrop1.wav".AsmSoundRes());
            return new LootSceneObject(Global.GameState.Player, this, this.Item.Name);
        }
    }
}
