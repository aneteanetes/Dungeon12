using Dungeon12.Classes;
using Dungeon.Drawing;
using Dungeon12.Items.Enums;
using Dungeon12.Items.Types;
using Dungeon12.Loot;
using Dungeon.Types;
using Force.DeepCloner;
using System;
using System.Collections.Generic;
using System.Text;
using Dungeon;

namespace Dungeon12.Items
{
    public class ItemGenerator : LootGenerator
    {
        public override Item Generate()
        {
            var additionalEquipments = Global.GameState.Equipment.AdditionalEquipments;
            var statEqip = additionalEquipments[RandomDungeon.Range(0, Global.GameState.Equipment.AdditionalEquipments.Count-1)].DeepClone();
            var values = statEqip.StatValues.Values;
            statEqip.StatProperties.ForEach((x, i) =>
            {
                values.Add(RandomDungeon.Range(2, 10));
            });

            return new Weapon
            {
                Tileset = "Dungeon12.Resources.Images.Items.Weapons.OneHand.Swords.TrainerSword.png",
                TileSetRegion = new Rectangle()
                {
                    X = 0,
                    Y = 0,
                    Width = 32,
                    Height = 96
                },
                Name = "Тренировочный меч",
                InventorySize = new Point(1, 3),
                Rare = Rarity.Rare,
                Cost = 50,
                BaseStats = new List<Equipment>()
                {
                    new BaseStatEquip()
                    {
                        StatName="Урон",
                        StatProperties=new List<string>() { "MinDMG","MaxDMG" },
                        StatValues=new List<long>(){ 1,3 },
                        Color= new DrawColor(System.ConsoleColor.DarkYellow)
                    },
                    statEqip
                }
            };
        }
    }
}
