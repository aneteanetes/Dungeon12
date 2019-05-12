namespace Rogue.Drawing.SceneObjects.Common
{
    using System;
    using Rogue.Abilities;
    using Rogue.Abilities.Enums;
    using Rogue.Abilities.Scaling;
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Map;
    using Rogue.Map.Objects;

    public class SkillControl : HandleSceneControl
    {
        public override bool AbsolutePosition => true;

        private readonly Ability ability;

        private readonly GameMap gameMap;

        private readonly Avatar avatar;

        public SkillControl(GameMap gameMap, Avatar avatar, Ability ability, AbilityPosition abilityPosition)
        {
            //this.key = key;
            this.avatar = avatar;
            this.gameMap = gameMap;
            this.ability = ability;

            if (ability == null)
            {
                this.ability = new EmptyAbility(abilityPosition);
            }

            var a = this.ability;

            var abilControl = new ImageControl(IsBig ? a.Image_B : a.Image);
            this.AddChild(abilControl);

            this.Image = SquareTexture(false);
        }

        public override double Width => IsBig ? 3 : 2;
        public override double Height => IsBig ? 3 : 2;

        public Action OnClick { get; set; }

        private string SquareTexture(bool focus)
        {
            var big = IsBig
                ? "B"
                : "";

            var f = focus
                ? "_f"
                : "";

            return $"Rogue.Resources.Images.ui.square{big}{f}.png";
        }

        public bool IsBig => this.ability.AbilityPosition == Rogue.Abilities.Enums.AbilityPosition.Left
            || this.ability.AbilityPosition == Rogue.Abilities.Enums.AbilityPosition.Right;

        public override string Image { get; set; }

        public override void Click(PointerArgs args)
        {
            Cast();
        }

        public override void GlobalClick(PointerArgs args)
        {
            if (args.MouseButton.ToString() == this.ability.AbilityPosition.ToString())
            {
                Cast();
            }

            var t = new System.Timers.Timer(200);
            t.AutoReset = false;
            t.Elapsed += (x, y) => this.Unfocus();
            t.Start();
        }

        public override void Focus()
        {
            this.Image = SquareTexture(true);
        }

        public override void Unfocus()
        {
            this.Image = SquareTexture(false);
        }

        public override void KeyDown(Key key, KeyModifiers modifier)
        {
            Cast();
        }

        private void Cast()
        {
            if (this.ability.CastAvailable(avatar))
            {
                this.ability.Cast(gameMap, avatar);                
                this.Focus();
            }
        }

        public override void KeyUp(Key key, KeyModifiers modifier)
        {
            this.Unfocus();
        }        

        private Key key = Key.None;

        protected override Key[] KeyHandles => new Key[] { key };

        private class EmptyAbility : Ability
        {
            public EmptyAbility(AbilityPosition abilityPosition)
            {
                this.AbilityPosition = abilityPosition;
            }

            public override int Position => -1;

            public override string Name => "";

            public override ScaleRate Scale => null;

            public override bool CastAvailable(Avatar avatar) => false;

            public override AbilityPosition AbilityPosition { get; }
        }

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.Click,
            ControlEventType.GlobalClick,
            ControlEventType.Focus
        };
    }
}