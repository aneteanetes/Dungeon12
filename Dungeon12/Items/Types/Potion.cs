namespace Dungeon12.Items.Types
{
    using Dungeon;
    using Dungeon12.Items.Enums;
    using Dungeon12.SceneObjects;
    using System.Collections.Generic;

    public class Potion : Item
    {
        private int _healing;

        public Potion(int hitpoints)
        {
            Name = "Зелье";
            Tileset = $"Items/Potions/1.gif".AsmImgRes();
            _healing = hitpoints;
            this.BaseStats.Add(new BaseStatEquip()
            {
                StatName = "Исцеление",
                StatProperties = new List<string>() { "HitPoints" },
                StatValues = new List<long>() { hitpoints },
                Color = Stats.Health.Color()
            });
        }

        public override ItemKind Kind => ItemKind.Potion;

        public override Rarity Rare => Rarity.Legendary;

        public override bool Stackable => true;

        public override int QuantityMax => 20;


        public override void Use()
        {
            var @char = Global.GameState.Character;
            @char.HitPoints += _healing;
            @char.Backpack.Remove(this, @char);
            Global.AudioPlayer.Effect("potion.wav".AsmSoundRes());

            Toast.Show($"Исцелено: {_healing}");
        }
    }
}