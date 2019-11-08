namespace Dungeon12.Drawing.SceneObjects.Main.CharacterBar
{
    using Dungeon.Control.Keys;
    using Dungeon12.Drawing.SceneObjects.Main.CharacterInfo;
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Map;
    using Dungeon.View.Interfaces;
    using System;
    using Dungeon;
    using Dungeon.Drawing.SceneObjects;
    using System.Collections.Generic;
    using Dungeon12.SceneObjects;
    using Dungeon.Control;

    public class TorchButton : SlideComponent
    {
        public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        private Player playerSceneObject;

        public TorchButton(Player playerSceneObject) : base("Факел (F)")
        {
            this.playerSceneObject = playerSceneObject;

            this.Height = 1;
            this.Width = 1;

            this.AddChild(new ImageControl("Dungeon12.Resources.Images.ui.player.torch.png")
            {
                CacheAvailable = false,
                Height = 1,
                Width = 1,
            });

            this.Image = SquareTexture(false);
        }

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
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

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold) => playerSceneObject.Torchlight();

        public override void Click(PointerArgs args) => playerSceneObject.Torchlight();
        
    }
}