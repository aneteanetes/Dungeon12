namespace Rogue.Drawing.SceneObjects.Main.Character
{
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Drawing.SceneObjects.UI;

    public class CharacterInfoWindow : DraggableControl
    {
        protected override Key[] OverrideKeyHandles => new Key[] { Key.C };

        public CharacterInfoWindow()
        {
            this.Image = "Rogue.Resources.Images.ui.vertical_title(17x12).png";

            this.Height = 17;
            this.Width = 12;

            this.Left = 4.5;
            this.Top = 2;

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
                Top = 4,
                Left = 2
            });

            this.AddChild(new ItemWear(Items.Enums.ItemKind.OffHand)
            {
                Top = 4,
                Left = 8
            });
        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.C)
            {
                base.KeyDown( Key.Escape, modifier, hold);
            }

            base.KeyDown(key, modifier, hold);
        }
    }
}