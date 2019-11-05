namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo
{
    using Dungeon;
    using Dungeon.Classes;
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Drawing.SceneObjects.UI;
    using Dungeon.Map;
    using Dungeon.SceneObjects;
    using Dungeon.View.Interfaces;
    using Dungeon12.Drawing.SceneObjects.Inventories;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CharacterInfoWindow : DraggableControl<CharacterInfoWindow>
    {
        protected override Key[] OverrideKeyHandles => new Key[] { Key.C, Key.I };

        private PlayerSceneObject playerSceneObject;
        public Inventory Inventory { get; }
        private bool selfclose = true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameMap"></param>
        /// <param name="playerSceneObject"></param>
        /// <param name="showEffects"></param>
        /// <param name="statBtn"></param>
        /// <param name="selfClose">Вообще я уверен что оставлял там возможность отрубать биндинги, но похуй пока что, экспресс разработка</param>
        public CharacterInfoWindow(GameMap gameMap, PlayerSceneObject playerSceneObject, Action<List<ISceneObject>> showEffects, bool statBtn = true,bool selfClose=true)
        {
            playerSceneObject.BlockMouse = true;
            this.Destroy += () => playerSceneObject.BlockMouse = false;
            this.playerSceneObject = playerSceneObject;


            this.Height = 17;
            this.Width = 12;

            this.Left = 3.5;
            this.Top = 2;

            this.selfclose = selfClose;

            if (statBtn)
                this.AddChild(new StatsButton(OpenStats, showEffects)
                {
                    Top = 2,
                    Left = 10.5
                });


            Inventory = new Inventory(playerSceneObject, playerSceneObject.Avatar.Character.Backpack)
            {
                Top = 9.45,
                Left = 0.55
            };
            AddItemWear(Inventory, playerSceneObject);

            Inventory.ItemWears = this.Children.Where(x => x.GetType() == typeof(ItemWear)).Cast<ItemWear>().ToArray();
            Inventory.Refresh();

            this.AddChild(Inventory);

            if (selfClose)
            {
                this.AddChild(new CharacterInfoDropItemMask(OnDropInventoryItem));

                this.AddChild(new InventoryDropItemMask(playerSceneObject, Inventory, gameMap)
                {
                    Left = -this.ComputedPosition.X,
                    Top = -this.ComputedPosition.Y
                });
            }

            FillData(playerSceneObject);

            if (statBtn)
                OpenStats();

            this.Image = "Dungeon12.Resources.Images.ui.infocharacter.png";
        }

        private void OnDropInventoryItem(InventoryItem item)
        {
            Global.DrawClient.Drop();
            this.Inventory.Refresh();
        }

        private Dictionary<string, TextControl> updateableStats = new Dictionary<string, TextControl>();

        private class StatContainer : SceneObject
        {
            TextControl _textControl;
            ClassStat _classStat;

            public StatContainer(ClassStat classStat)
            {
                _classStat = classStat;
                this.Width = 2;
                this.Height = 1;

                var img = this.AddChildImageCenter(new ImageControl("Dungeon12.Resources.Images.ui.stats.attack.png")
                {
                    Height = .5,
                    Width = .5,
                    AbsolutePosition = true,
                    CacheAvailable = false
                });
                img.Left = -.5;

                _textControl = this.AddTextCenter(new DrawText(classStat.StatValues, classStat.Color).Montserrat());
            }

            public override void Update()
            {
                _textControl.Text.SetText(_classStat.StatValues);
            }
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

            updateableStats.Add("Class", @class);

            var exp = this.AddTextCenter(new DrawText($"Опыт: {character.EXP}/{character.MaxExp}", new DrawColor(ConsoleColor.White)).Montserrat());
            exp.Left = .5;
            exp.Top = 2.5;

            updateableStats.Add("Exp", exp);           

            var statGroup = character.ClassStats.GroupBy(s => s.Group);

            var statLeft = statGroup.First();
            var clsStatTop = 7.5;
            foreach (var stat in statLeft)
            {
                this.AddChild(new StatContainer(stat)
                {
                    Top = clsStatTop,
                    Left = 2,
                });
                //this.AddChild(new ImageControl("Dungeon12.Resources.Images.ui.stats.attack.png")
                //{
                //    Height = 0.5,
                //    Width = 0.5,
                //    Top = clsStatTop,
                //    Left = 2,
                //    AbsolutePosition = true,
                //    CacheAvailable = false
                //});

                //var clsstatLeft = this.AddTextCenter(new DrawText(stat.StatValues, stat.Color).Montserrat());
                //clsstatLeft.Left = 2.75;
                //clsstatLeft.Top = clsStatTop;

                //updateableStats.Add("ClsStat" + character.ClassStats.IndexOf(stat), clsstatLeft);

                clsStatTop += .5;
            }

            var statRight = statGroup.Last();
            clsStatTop = 7.5;
            foreach (var stat in statRight)
            {
                this.AddChild(new StatContainer(stat)
                {
                    Top = clsStatTop,
                    Left = 8,
                });

                //this.AddChild(new ImageControl("Dungeon12.Resources.Images.ui.stats.attack.png")
                //{
                //    Height = 0.5,
                //    Width = 0.5,
                //    Top = clsStatTop,
                //    Left = 8,
                //    AbsolutePosition = true,
                //    CacheAvailable = false
                //});

                //var clsstatLeft = this.AddTextCenter(new DrawText(stat.StatValues, stat.Color).Montserrat());
                //clsstatLeft.Left = 8.75;
                //clsstatLeft.Top = clsStatTop;

                //updateableStats.Add("ClsStat" + character.ClassStats.IndexOf(stat), clsstatLeft);

                clsStatTop += .5;
            }

            var goldImg = "Dungeon12.Resources.Images.ui.stats.gold.png";
            var goldMeasure = this.MeasureImage(goldImg);

            var goldLeft = this.Width / 2 - ((goldMeasure.X * 0.8) / 32 / 2);
            this.AddChild(new ImageControl("Dungeon12.Resources.Images.ui.stats.gold.png")
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

            updateableStats.Add("Gold", gold);
        }
                
        public override void Update()
        {
            var character = playerSceneObject.Avatar.Character;
            updateableStats["Class"].Text.SetText(character.ClassName);
            updateableStats["Exp"].Text.SetText($"Опыт: {character.EXP}/{character.MaxExp}");
            updateableStats["Gold"].Text.SetText($"{character.Gold}");
        }

        private void AddItemWear(Inventory inventory, PlayerSceneObject playerSceneObject)
        {
            this.AddChild(new ItemWear(inventory, playerSceneObject.Avatar.Character, Dungeon.Items.Enums.ItemKind.Helm)
            {
                Top = 2,
                Left = 5
            });

            this.AddChild(new ItemWear(inventory, playerSceneObject.Avatar.Character, Dungeon.Items.Enums.ItemKind.Armor)
            {
                Top = 4.5,
                Left = 5
            });

            this.AddChild(new ItemWear(inventory, playerSceneObject.Avatar.Character, Dungeon.Items.Enums.ItemKind.Boots)
            {
                Top = 7,
                Left = 5
            });

            this.AddChild(new ItemWear(inventory, playerSceneObject.Avatar.Character, Dungeon.Items.Enums.ItemKind.Weapon)
            {
                Top = 3.5,
                Left = 2
            });

            this.AddChild(new ItemWear(inventory, playerSceneObject.Avatar.Character, Dungeon.Items.Enums.ItemKind.OffHand)
            {
                Top = 3.5,
                Left = 8
            });
        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (this.selfclose)
            {
                if (key == Key.C || key == Key.I)
                {
                    base.KeyDown(Key.Escape, modifier, hold);
                }

                base.KeyDown(key, modifier, hold);
            }
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

            private readonly Action open;

            public StatsButton(Action open, Action<List<ISceneObject>> showEffects) : base("Характеристики", showEffects)
            {
                this.open = open;

                this.Height = 1;
                this.Width = 1;

                this.AddChild(new ImageControl("Dungeon12.Resources.Images.ui.additional.png")
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

                return $"Dungeon12.Resources.Images.ui.square{f}.png";
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

    }
}