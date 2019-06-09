namespace Rogue.Drawing.SceneObjects.Main.CharacterInfo
{
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Items.Enums;

    public class ItemWear : DropableControl
    {
        private string borderImage = string.Empty;

        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        public ItemWear(ItemKind itemKind)
        {
            var tall = itemKind == ItemKind.Weapon || itemKind == ItemKind.OffHand;

            this.borderImage = tall
                ? "Rogue.Resources.Images.ui.squareWeapon"
                : "Rogue.Resources.Images.ui.square";

            this.Width = 2;
            this.Height = tall
                ? 4
                : 2;

            this.Image = SquareTexture();
        }

        private string SquareTexture(bool focus=false)
        {
            var f = focus
                ? "_f"
                : "";

            return $"{borderImage}{f}.png";
        }

        public override void Focus()
        {
            this.Image = SquareTexture(true);
            base.Focus();
        }

        public override void Unfocus()
        {
            this.Image = SquareTexture();
            base.Unfocus();
        }
    }
}