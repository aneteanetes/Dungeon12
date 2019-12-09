namespace Dungeon12.Loot
{
    using Dungeon12.Classes;
    using Dungeon.Drawing;
    using Dungeon12.Items;
    using Dungeon12.Items.Enums;
    using Dungeon12.Items.Types;
    using Dungeon;

    public abstract class LootGenerator : ITrigger<Item, string[]>
    {
        protected string[] Arguments { get; private set; } = new string[0];

        public void SetArguments(string[] arguments) => Arguments = arguments;

        Item ITrigger<Item, string[]>.Trigger(string[] arg1) => default;

        public abstract Item Generate();

        //public static LootContainer Generate()
        //{
        //    return new LootContainer()
        //    {
        //        Gold = RandomDungeon.Next(0, 20),
        //        Items = new System.Collections.Generic.List<Item>()
        //        {
        //            new Weapon()
        //            {
        //                Tileset = "Dungeon12.Resources.Images.Items.Weapons.OneHand.Swords.TrainerSword.png",
        //                TileSetRegion = new Dungeon.Types.Rectangle()
        //                {
        //                    X = 0,
        //                    Y = 0,
        //                    Width = 32,
        //                    Height = 96
        //                },
        //                Name = "Меч новичка",
        //                InventorySize = new Dungeon.Types.Point(1, 3),
        //                Rare =  Rarity.Set,
        //                BaseStats=new System.Collections.Generic.List<Equipment>()
        //                {
        //                    new BaseStatEquip()
        //                    {
        //                        StatName="Урон",
        //                        StatProperties=new System.Collections.Generic.List<string>() { "MinDMG","MaxDMG" },
        //                        StatValues=new System.Collections.Generic.List<long>(){ 1,3 },
        //                        Color= new DrawColor(System.ConsoleColor.DarkYellow)
        //                    },
        //                    new BaseStatEquip()
        //                    {
        //                        StatName="Сила атаки",
        //                        StatProperties= "AttackPower".InList(),
        //                        StatValues=5L.InList(),
        //                        Color= new DrawColor(System.ConsoleColor.Cyan)
        //                    }
        //                },
        //                Additional=new MagicFindEquip().InList<Equipment>(),
        //                ClassStats=new System.Collections.Generic.List<Equipment>()
        //                {
        //                    new ParryEquip(),
        //                },
        //                ItemSetName="Набор новичка",
        //                ItemSet=new System.Collections.Generic.List<Equipment>()
        //                {
        //                    new NoviceEquip()
        //                }
        //            },
        //            new OffHand()
        //            {
        //                Tileset = "Dungeon12.Resources.Images.Items.Offhands.Shields.Tall.DragonShield.png",
        //                TileSetRegion = new Dungeon.Types.Rectangle()
        //                {
        //                    X = 0,
        //                    Y = 0,
        //                    Width = 64,
        //                    Height = 64
        //                },
        //                Name = "Щит Новичка",
        //                InventorySize = new Dungeon.Types.Point(2, 4),
        //                Rare = Rarity.Set,
        //                BaseStats=new System.Collections.Generic.List<Equipment>()
        //                {
        //                    new BaseStatEquip()
        //                    {
        //                        StatName="Защита",
        //                        StatProperties= "Defence".InList(),
        //                        StatValues=3L.InList(),
        //                        Color= new DrawColor(System.ConsoleColor.DarkCyan)
        //                    },
        //                    new BaseStatEquip()
        //                    {
        //                        StatName="Барьер",
        //                        StatProperties= "Barrier".InList(),
        //                        StatValues=2L.InList(),
        //                        Color= new DrawColor(System.ConsoleColor.DarkMagenta)
        //                    }
        //                },
        //                ClassStats=new System.Collections.Generic.List<Equipment>()
        //                {
        //                    new BlockEquip(),
        //                },
        //                ItemSetName="Набор новичка",
        //                ItemSet=new System.Collections.Generic.List<Equipment>()
        //                {
        //                    new NoviceEquip()
        //                }
        //            }
        //        }
        //    };
        //}

        private class ParryEquip : Equipment
        {
            public override string Title => $"+12 Паррирование";

            public void Apply(Character character)
            {

            }

            public void Discard(Character character)
            {

            }

            protected override void CallApply(dynamic obj) => this.Apply(obj);
            protected override void CallDiscard(dynamic obj) => this.Discard(obj);
        }

        private class NoviceEquip : Equipment
        {
            public override string Title => $"+5 Паррирование";

            public void Apply(Character character)
            {

            }

            public void Discard(Character character)
            {

            }

            protected override void CallApply(dynamic obj) => this.Apply(obj);
            protected override void CallDiscard(dynamic obj) => this.Discard(obj);
        }

        public class BlockEquip : Equipment
        {
            public override string Title => $"Блок: 8";

            public void Apply(Character character)
            {

            }

            public void Discard(Character character)
            {

            }

            protected override void CallApply(dynamic obj) => this.Apply(obj);
            protected override void CallDiscard(dynamic obj) => this.Discard(obj);
        }

        public static Item GenerateWeapon() => new ItemGenerator().Generate();

    }
}