namespace Rogue.Drawing.SceneObjects.Common
{
    using System;
    using Rogue.Abilities;
    using Rogue.Control.Keys;
    using Rogue.Map;
    using Rogue.Map.Objects;

    public class SkillControl : HandleSceneControl
    {
        public override bool AbsolutePosition => true;

        private readonly Ability ability;

        private readonly GameMap gameMap;

        private readonly Avatar avatar;

        public SkillControl(Key key, GameMap gameMap, Avatar avatar, Ability ability)
        {
            this.key = key;
            this.avatar = avatar;
            this.gameMap = gameMap;
            this.ability = ability;

            var abilControl = new ImageControl(this.ability.Image)
            {
                Width = 1.4,
                Height = 1.4,
                Left=0.3,
                Top=0.3
            };
            this.AddChild(abilControl);
        }

        public override double Width { get => 2; set { } }
        public override double Height { get => 2; set { } }

        public Action OnClick { get; set; }

        public override string Image { get; set; } = "Rogue.Resources.Images.ui.square.png";

        public override void Click()
        {
            OnClick?.Invoke();
        }

        public override void Focus()
        {
            this.Image = "Rogue.Resources.Images.ui.square_f.png";
        }

        public override void Unfocus()
        {
            this.Image = "Rogue.Resources.Images.ui.square.png";
        }

        public override void KeyDown(Key key, KeyModifiers modifier)
        {
            this.ability.Use(gameMap, avatar);
            this.Focus();
        }

        public override void KeyUp(Key key, KeyModifiers modifier)
        {
            this.Unfocus();
        }

        private Key key = Key.None;

        protected override Key[] KeyHandles => new Key[] { key };
    }
}