using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Drawing.SceneObjects.Main.CharacterBar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.SceneObjects.Main.CharacterInfo.Stats
{
    public class OpenFractionButton : EmptyTooltipedSceneObject
    {
        public override bool CacheAvailable => false;
        public override bool AbsolutePosition => true;

        private readonly Action open;

        public OpenFractionButton(Action open,string tooltipText) : base(tooltipText)
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

        public override void Click(PointerArgs args) => open?.Invoke();
    }
}
