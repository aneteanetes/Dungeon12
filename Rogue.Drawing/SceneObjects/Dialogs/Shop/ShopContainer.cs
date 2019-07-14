namespace Rogue.Drawing.SceneObjects.Dialogs.Shop
{
    using Rogue.Control.Keys;
    using Rogue.Drawing.SceneObjects.Main.CharacterInfo;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Map;
    using Rogue.View.Interfaces;
    using System;

    public class ShopContainer : DraggableControl<ShopContainer>
    {
        public override int Layer => 50;

        public override bool AbsolutePosition => true;

        protected override Key[] OverrideKeyHandles => new Key[] { Key.Escape };

        public ShopContainer(string title, PlayerSceneObject playerSceneObject, Merchants.Merchant shop, Action<ISceneObject> destroyBinding, Action<ISceneObjectControl> controlBinding, GameMap gameMap)
        {
            Global.FreezeWorld = this;

            var charInfo = new CharacterInfoWindow(gameMap, playerSceneObject, this.ShowEffects, false,false)
            {
                Left = 16 + 5,
                DisableDrag = true
            };
            charInfo.Top = 0;

            var shopWindow = new ShopWindow(title, shop, playerSceneObject)
            {
                Left = 5,
            };

            this.Width = 28;
            this.Height = 17;

            this.Top = 2;

            this.AddChild(shopWindow);
            this.AddChild(charInfo);
        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold) => Close();

        private void Close()
        {
            this.Destroy?.Invoke();
            Global.FreezeWorld = null;
        }
    }
}