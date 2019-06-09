namespace Rogue.Drawing.SceneObjects.Main.Character
{
    using Rogue.Abilities;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SkillsWindow : DraggableControl
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        public SkillsWindow(PlayerSceneObject playerSceneObject)
        {
            playerSceneObject.BlockMouse = true;
            this.Destroy += () => playerSceneObject.BlockMouse = false;

            this.Image = "Rogue.Resources.Images.ui.vertical_title(17x12).png";

            this.Height = 17;
            this.Width = 12;

            this.Left = 22.5;
            this.Top = 2;

            var abils = playerSceneObject.GetAbilities();

            var left = abils.FirstOrDefault(x => x.AbilityPosition == Rogue.Abilities.Enums.AbilityPosition.Left);
            var right = abils.FirstOrDefault(x => x.AbilityPosition == Rogue.Abilities.Enums.AbilityPosition.Right);
            var q = abils.FirstOrDefault(x => x.AbilityPosition == Rogue.Abilities.Enums.AbilityPosition.Q);
            var e = abils.FirstOrDefault(x => x.AbilityPosition == Rogue.Abilities.Enums.AbilityPosition.E);

            this.AddChild(new SkillButton(left, null,this.ShowEffects,true));
            this.AddChild(new SkillButton(right, null, this.ShowEffects)
            {
                Left=3
            });
            this.AddChild(new SkillButton(q, null, this.ShowEffects)
            {
                Left = 6
            });
            this.AddChild(new SkillButton(e, null, this.ShowEffects)
            {
                Left = 9
            });
        }
        
        protected override Key[] OverrideKeyHandles => new Key[] { Key.V, Key.X };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.V || key== Key.X)
            {
                base.KeyDown(Key.Escape, modifier, hold);
            }

            base.KeyDown(key, modifier, hold);
        }

        private class SkillButton : TooltipedSceneObject
        {
            private bool active = false;

            public override bool CacheAvailable => false;
            public override bool AbsolutePosition => true;

            private PlayerSceneObject playerSceneObject;
            private readonly Action open;
            private bool disabled;

            public SkillButton(Ability ability, Action open,Action<List<ISceneObject>> showEffects, bool active=false) : base("Характеристики", showEffects)
            {
                this.active = active;
                this.disabled = ability==null;
                this.open = open;

                this.Height = 2;
                this.Width = 3;

                if (ability != null)
                {
                    this.AddTextCenter(new DrawText(ability.AbilityPosition.ToString()));
                }

                this.Image = SquareTexture(false);
            }

            private string SquareTexture(bool focus)
            {
                if(disabled)
                    return $"Rogue.Resources.Images.ui.squareWeapon_h_d.png";

                var f = focus || active
                    ? "_f"
                    : "";

                return $"Rogue.Resources.Images.ui.squareWeapon_h{f}.png";
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

            public override void KeyDown(Key key, KeyModifiers modifier, bool hold) => open?.Invoke();

            public override void Click(PointerArgs args) => open?.Invoke();
        }
    }
}
