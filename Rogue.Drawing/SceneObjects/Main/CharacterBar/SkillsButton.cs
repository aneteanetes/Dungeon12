namespace Rogue.Drawing.SceneObjects.Main.CharacterBar
{
    using Rogue.Control.Keys;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public class SkillsButton : TooltipedSceneObject
    {
        public override bool AbsolutePosition => true;

        public SkillsButton(Action<List<ISceneObject>> showEffects) : base("Навыки", showEffects)
        {
            this.Height = 1.5;
            this.Width = 1.5;

            this.AddChild(new ImageControl("Rogue.Resources.Images.ui.player.skills.png")
            {
                CacheAvailable = true,
                Height = 1.5,
                Width = 1.5,
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
            Key.X
        };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            Console.WriteLine("skills opened");
            base.KeyDown(key, modifier, hold);
        }
    }
}