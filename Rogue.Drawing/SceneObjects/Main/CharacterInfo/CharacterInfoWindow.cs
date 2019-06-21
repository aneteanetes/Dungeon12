namespace Rogue.Drawing.SceneObjects.Main.CharacterInfo
{
    using Force.DeepCloner;
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Map;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CharacterInfoWindow : DraggableControl
    {
        protected override Key[] OverrideKeyHandles => new Key[] { Key.C, Key.I };

        private PlayerSceneObject playerSceneObject;
        private Inventory inventory;

        public CharacterInfoWindow(GameMap gameMap, PlayerSceneObject playerSceneObject, Action<List<ISceneObject>> showEffects)
        {
            playerSceneObject.BlockMouse = true;
            this.Destroy += () => playerSceneObject.BlockMouse = false;
            this.playerSceneObject = playerSceneObject;

            this.Image = "Rogue.Resources.Images.ui.infocharacter.png";

            this.Height = 17;
            this.Width = 12;

            this.Left = 3.5;
            this.Top = 2;

            this.AddChild(new StatsButton(OpenStats, showEffects)
            {
                Top = 2,
                Left = 10.5
            });


            inventory = new Inventory(this.ZIndex, playerSceneObject.Avatar.Character.Backpack)
            {
                Top = 9.45,
                Left = 0.55
            };
            AddItemWear(inventory, playerSceneObject);

            inventory.ItemWears = this.Children.Where(x => x.GetType() == typeof(ItemWear)).Cast<ItemWear>().ToArray();
            inventory.Refresh();

            this.AddChild(inventory);

            this.AddChild(new CharacterInfoDropItemMask(OnDropInventoryItem));

            this.AddChild(new InventoryDropItemMask(playerSceneObject, inventory, gameMap)
            {
                Left = -this.Left,
                Top = -this.Top
            });

            FillData(playerSceneObject);

            OpenStats();
        }

        private void OnDropInventoryItem(InventoryItem item)
        {
            Global.DrawClient.Drop();
            this.inventory.Refresh();
        }

        private void FillData(PlayerSceneObject playerSceneObject)
        {
            var character = playerSceneObject.Avatar.Character;

            var txt = this.AddTextCenter(new DrawText(character.Name), true, false);
            txt.Top += 0.2;

            var origin = this.AddTextCenter(new DrawText(character.Origin.ToDisplay(), new DrawColor(ConsoleColor.DarkYellow)).Montserrat());
            origin.Left = 7.3;
            origin.Top = 2;


            var @class = this.AddTextCenter(new DrawText(character.ClassName, new DrawColor(ConsoleColor.Cyan)).Montserrat());
            @class.Left = 0.5;
            @class.Top = 2;

            var exp = this.AddTextCenter(new DrawText($"Опыт: {character.EXP}/{character.MaxExp}", new DrawColor(ConsoleColor.White)).Montserrat());
            exp.Left = .5;
            exp.Top = 2.5;

            this.AddChild(new ImageControl("Rogue.Resources.Images.ui.stats.attack.png")
            {
                Height = 0.5,
                Width = 0.5,
                Top = 8,
                Left = 2,
                AbsolutePosition = true,
                CacheAvailable = false
            });

            var dmgTxt = this.AddTextCenter(new DrawText($"{character.MinDMG} - {character.MaxDMG}", new DrawColor(230, 118, 37, 255)).Montserrat());
            dmgTxt.Left = 2.75;
            dmgTxt.Top = 7.95;


            this.AddChild(new ImageControl("Rogue.Resources.Images.ui.stats.defence.png")
            {
                Height = 0.5,
                Width = 0.5,
                Top = 8,
                Left = 8.5,
                AbsolutePosition = true,
                CacheAvailable = false
            });

            var arm = this.AddTextCenter(new DrawText($"{character.Defence}", new DrawColor(37, 223, 230, 255)).Montserrat());
            arm.Left = 8.5 + .75;
            arm.Top = 7.95;


            var goldImg = "Rogue.Resources.Images.ui.stats.gold.png";
            var goldMeasure = this.MeasureImage(goldImg);

            var goldLeft = this.Width / 2 - ((goldMeasure.X * 0.8) / 32 / 2);
            this.AddChild(new ImageControl("Rogue.Resources.Images.ui.stats.gold.png")
            {
                Height = 0.85,
                Width = 0.85,
                Top = 15.75,
                Left = goldLeft - 0.85,
                AbsolutePosition = true,
                CacheAvailable = false
            });

            var gold = this.AddTextCenter(new DrawText($"{character.Gold}", new DrawColor(255, 243, 119, 255)).Montserrat());
            gold.Left = goldLeft + 0.3;
            gold.Top = 15.75;
        }

        private void AddItemWear(Inventory inventory, PlayerSceneObject playerSceneObject)
        {
            this.AddChild(new ItemWear(inventory, playerSceneObject.Avatar.Character, Items.Enums.ItemKind.Helm)
            {
                Top = 2,
                Left = 5
            });

            this.AddChild(new ItemWear(inventory, playerSceneObject.Avatar.Character, Items.Enums.ItemKind.Armor)
            {
                Top = 4.5,
                Left = 5
            });

            this.AddChild(new ItemWear(inventory, playerSceneObject.Avatar.Character, Items.Enums.ItemKind.Boots)
            {
                Top = 7,
                Left = 5
            });

            this.AddChild(new ItemWear(inventory, playerSceneObject.Avatar.Character, Items.Enums.ItemKind.Weapon)
            {
                Top = 3.5,
                Left = 2
            });

            this.AddChild(new ItemWear(inventory, playerSceneObject.Avatar.Character, Items.Enums.ItemKind.OffHand)
            {
                Top = 3.5,
                Left = 8
            });
        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.C || key == Key.I)
            {
                base.KeyDown(Key.Escape, modifier, hold);
            }

            base.KeyDown(key, modifier, hold);
        }

        private StatsInfo statsInfo = null;

        private void OpenStats()
        {
            if (statsInfo != null)
            {
                statsInfo.Destroy?.Invoke();
                this.RemoveChild(statsInfo);
                statsInfo = null;
            }
            else
            {
                statsInfo = new StatsInfo(playerSceneObject.Avatar.Character)
                {
                    Left = this.Width + 0.5
                };
                this.AddChild(statsInfo);
            }
        }

        private class StatsButton : TooltipedSceneObject
        {
            public override bool CacheAvailable => false;
            public override bool AbsolutePosition => true;

            private PlayerSceneObject playerSceneObject;
            private readonly Action open;

            public StatsButton(Action open, Action<List<ISceneObject>> showEffects) : base("Характеристики", showEffects)
            {
                this.open = open;

                this.Height = 1;
                this.Width = 1;

                this.AddChild(new ImageControl("Rogue.Resources.Images.ui.additional.png")
                {
                    AbsolutePosition = true,
                    CacheAvailable = false,
                    Height = 1,
                    Width = 1,
                });

                this.Image = SquareTexture(false);
            }

            private string SquareTexture(bool focus)
            {
                var f = focus
                    ? "_f"
                    : "";

                return $"Rogue.Resources.Images.ui.square{f}.png";
            }

            public override void Focus()
            {
                this.Image = SquareTexture(true);
                base.Focus();
            }

            public override void Unfocus()
            {
                this.Image = SquareTexture(false);
                base.Unfocus();
            }

            protected override Key[] KeyHandles => new Key[]
            {
                Key.F
            };

            public override void KeyDown(Key key, KeyModifiers modifier, bool hold) => open?.Invoke();

            public override void Click(PointerArgs args) => open?.Invoke();
        }

        private class CharacterInfoDropItemMask : DropableControl<InventoryItem>
        {
            public override bool AbsolutePosition => true;

            public override bool CacheAvailable => true;

            Action<InventoryItem> OnDropDelegate;

            public CharacterInfoDropItemMask(Action<InventoryItem> onDropDelegate)
            {
                this.OnDropDelegate = onDropDelegate;
                this.Height = 17;
                this.Width = 12;
            }

            protected override void OnDrop(InventoryItem source) => OnDropDelegate?.Invoke(source);
        }

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
                if (source.DropProcessed==1)
                {
                    playerSceneObject.Avatar.Character.Backpack.Remove(source.Item);
                    inventory.Refresh();
                    AddLootToMap(source);
                }

                base.OnDrop(source);
            }

            private void AddLootToMap(InventoryItem source)
            {
                var lootItem = new Rogue.Map.Objects.Loot()
                {
                    Item = source.Item
                };

                lootItem.Location = gameMap.RandomizeLocation(playerSceneObject.Avatar.Location.DeepClone());
                lootItem.Destroy += () => gameMap.Map.Remove(lootItem);

                gameMap.Map.Add(lootItem);
                gameMap.PublishObject(lootItem);
            }
        }
    }
}