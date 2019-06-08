namespace Rogue.Drawing.SceneObjects.Main.Character
{
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public class CharacterInfoWindow : DraggableControl
    {
        public override bool Singleton => true;

        protected override Key[] OverrideKeyHandles => new Key[] { Key.C, Key.X };

        public CharacterInfoWindow(PlayerSceneObject playerSceneObject, Action<List<ISceneObject>> showEffects)
        {
            this.Image = "Rogue.Resources.Images.ui.infocharacter.png";

            this.Height = 17;
            this.Width = 12;

            this.Left = 3.5;
            this.Top = 2;

            this.AddChild(new StatsButton(OpenStats,showEffects)
            {
                Top = 2,
                Left = 10.5
            });

            this.AddChild(new ItemWear(Items.Enums.ItemKind.Helm)
            {
                Top=2,
                Left=5
            });

            this.AddChild(new ItemWear(Items.Enums.ItemKind.Helm)
            {
                Top = 4.5,
                Left = 5
            });

            this.AddChild(new ItemWear(Items.Enums.ItemKind.Helm)
            {
                Top = 7,
                Left = 5
            });

            this.AddChild(new ItemWear(Items.Enums.ItemKind.Weapon)
            {
                Top = 3.5,
                Left = 2
            });

            this.AddChild(new ItemWear(Items.Enums.ItemKind.OffHand)
            {
                Top = 3.5,
                Left = 8
            });

            this.AddChild(new Inventory()
            {
                Top=9.45,
                Left=0.55
            });

            OpenStats();
        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.C)
            {
                base.KeyDown( Key.Escape, modifier, hold);
            }
            else if (key== Key.X)
            {
                ShowEffects(new List<ISceneObject>()
                {
                    new SkillsWindow()
                });
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
                statsInfo = new StatsInfo()
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
                    AbsolutePosition=true,
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
                base.Focus();
            }

            protected override Key[] KeyHandles => new Key[]
            {
                Key.F
            };

            public override void KeyDown(Key key, KeyModifiers modifier, bool hold) =>open?.Invoke();

            public override void Click(PointerArgs args) => open?.Invoke();
        }
    }
}