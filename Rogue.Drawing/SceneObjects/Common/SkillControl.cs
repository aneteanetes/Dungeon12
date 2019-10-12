namespace Rogue.Drawing.SceneObjects.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rogue.Abilities;
    using Rogue.Abilities.Enums;
    using Rogue.Abilities.Scaling;
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.GUI;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Drawing.SceneObjects.Dialogs.NPC;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.Types;
    using Rogue.View.Interfaces;

    public class SkillControl : TooltipedSceneObject
    {
        private static bool interacting = false;

        public static bool CancelClick() => interacting = true;
        public static bool RestoreClick() => interacting = false;

        public override bool AbsolutePosition => true;

        private readonly Ability ability;

        private readonly GameMap gameMap;

        private readonly Avatar avatar;

        private readonly PlayerSceneObject player;

        private readonly ImageControl abilControl;

        private readonly AbilityPosition abilityPosition;

        private Action<ISceneObject> destroyBinding;

        private Action<ISceneObjectControl> controlBinding;

        private bool empty = false;

        public SkillControl(GameMap gameMap, PlayerSceneObject player, Ability ability, AbilityPosition abilityPosition, Action<List<ISceneObject>> abilityEffects, Action<ISceneObject> destroyBinding, Action<ISceneObjectControl> controlBinding)
            : base(ability?.Name, abilityEffects)
        {
            this.controlBinding = controlBinding;
            this.destroyBinding = destroyBinding;
            this.abilityPosition = abilityPosition;
            this.avatar = player.Avatar;
            this.player = player;
            this.gameMap = gameMap;
            this.ability = ability;

            if (abilityPosition == AbilityPosition.E)
            {
                key = Key.E;
            }
            if (abilityPosition == AbilityPosition.Q)
            {
                key = Key.Q;
            }

            if (ability == null)
            {
                empty = true;
                this.ability = new EmptyAbility(abilityPosition);
            }

            this.ability.OnCast += () =>
            {
                this.Image = SquareTexture(true);
                RemoveFocusImage();
            };

            var a = this.ability;

            abilControl = new ImageControl(IsBig ? a.Image_B : a.Image)
            {
                CacheAvailable = false
            };
            this.AddChild(abilControl);

            if (this.ability.CastType == AbilityCastType.Passive)
            {
                this.ability.CastCooldown(gameMap, avatar);
            }

            this.AddChild(new CooldownMask(this.ability));

            this.Image = SquareTexture(false);
        }

        private class CooldownMask : ImageControl
        {
            public override bool AbsolutePosition => true;
            public override bool CacheAvailable => false;

            public CooldownMask(Ability ability) : base(@"ui\square_transparent_mask.png".PathImage())
            {
                this.Mask = SceneObjects.ImageMask.Radial()
                    .BindAmount(() => ability.Cooldown?.Percent ?? 0f)
                    .BindVisible(() => ability.Cooldown?.IsActive ?? false);

                this.Mask.Color = new DrawColor(ConsoleColor.Black);
                this.Mask.Opacity = 0.5f;

                Global.DrawClient.CacheObject(this);
            }
        }

        public override double Width => IsBig ? 3 : 2;
        public override double Height => IsBig ? 3 : 2;

        public Action OnClick { get; set; }

        private string SquareTexture(bool focus)
        {
            if (empty || !abilControl.Visible)
                return $"Rogue.Resources.Images.ui.square_d.png";

            if (ability != null)
            {
                if (ability.CastType == AbilityCastType.Passive)
                {
                    if (ability.PassiveWorking)
                        return $"Rogue.Resources.Images.ui.square_f.png";
                    else
                        return $"Rogue.Resources.Images.ui.square_d.png";
                }
            }

            var big = IsBig
                ? "B"
                : "";

            var f = focus
                ? "_f"
                : "";

            return $"Rogue.Resources.Images.ui.square{big}{f}.png";
        }

        public bool IsBig => false;
        //this.ability.AbilityPosition == Rogue.Abilities.Enums.AbilityPosition.Left
        //    || this.ability.AbilityPosition == Rogue.Abilities.Enums.AbilityPosition.Right;

        private string image { get; set; }

        private bool SafeZoneInvisible => !ability.AvailableInSafeZone && gameMap.InSafe(avatar);

        public override string Image
        {
            get
            {
                if (SafeZoneInvisible)
                {
                    SafeMode = true;
                    var img = this.ability.Image;

                    switch (abilityPosition)
                    {
                        case AbilityPosition.Left:
                            this.TooltipText = "Поговорить";
                            img = $"Rogue.Resources.Images.ui.talk.png";
                            break;
                        case AbilityPosition.Right:
                            this.TooltipText = "Информация";
                            img = $"Rogue.Resources.Images.ui.info.png";
                            break;
                        default:
                            this.TooltipText = string.Empty;
                            this.Image = SquareTexture(false);
                            abilControl.Visible = false;
                            break;
                    }

                    abilControl.Image = img;
                }
                else
                {
                    SafeMode = false;
                    this.TooltipText = ability.Name;
                    abilControl.Visible = true;
                    abilControl.Image = this.ability.Image;
                    this.Image = SquareTexture(false);
                }

                if (holds || highlight || infocus)
                {
                    this.Image = SquareTexture(true);
                }

                return image;
            }
            set => image = value;
        }
        
        private bool holds = false;

        private bool highlight = false;

        private void GlobalSafeClick(PointerArgs args)
        {
            //var conversational = gameMap.Conversations(this.avatar).FirstOrDefault();

            //if (conversational != null)
            //{
            //    player.StopMovings();
            //    ShowEffects?.Invoke(new List<ISceneObject>
            //    {
            //        new NPCDialogue(conversational,destroyBinding,controlBinding)
            //    });
            //}
        }

        private bool InteractInterrupt()
        {
            return avatar.SafeMode;
        }

        public override void GlobalClick(PointerArgs args)
        {
            if (ability.TargetType == AbilityTargetType.Target)
                return;

            if (player.BlockMouse)
                return;

            if (args.MouseButton.ToString() != this.ability.AbilityPosition.ToString())
            {
                return;
            }

            if (args.MouseButton == MouseButton.Left)
            {
                if (InteractInterrupt())
                    return;
            }

            Cast(args);

            if (!ability.Hold || (SafeZoneInvisible && (abilityPosition== AbilityPosition.Left || abilityPosition== AbilityPosition.Right)))
            {
                RemoveFocusImage();
            }
            else
            {
                holds = true;
                highlight = true;
            }
        }

        private void RemoveFocusImage()
        {
            var t = new System.Timers.Timer(200);
            t.AutoReset = false;
            t.Elapsed += (x, y) => highlight = false;
            t.Start();
        }

        public override void GlobalClickRelease(PointerArgs args)
        {
            if (args.MouseButton.ToString() != this.ability.AbilityPosition.ToString())
            {
                return;
            }

            if (ability.Hold && holds)
            {
                ability.Release(gameMap, avatar);
                this.Image = SquareTexture(false);
                holds = false;
                highlight = false;
            }
        }

        private bool infocus = false;

        public override void Focus()
        {
            this.infocus = true;
            //if (!SafeZoneInvisible)
            //    this.Image = SquareTexture(true);

            base.Focus();
        }

        public override void Unfocus()
        {
            this.infocus = false;
            //if (!SafeZoneInvisible)
            //    this.Image = SquareTexture(false);

            base.Unfocus();
        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            Cast(null);
        }

        private void Cast(PointerArgs args)
        {
            if (interacting)
            {
                interacting = false;
                return;
            }

            if (ability.CastType == AbilityCastType.Passive)
                return;

            if (SafeZoneInvisible)
            {
                GlobalSafeClick(args);
                this.Image = SquareTexture(true);
                return;
            }

            if (player.TargetsInFocus.Count == 0)
            {
                if (this.ability.CastAvailableCooldown(avatar))
                {
                    this.ability.CastCooldown(gameMap, avatar);
                    this.highlight = true;
                    this.Image = SquareTexture(true);
                }
                else
                {
                    var pos = new Point(player.ComputedPosition.X, player.ComputedPosition.Y);
                    var effect = new PopupString("Невозможно использовать способность!", ConsoleColor.White, pos).InList<ISceneObject>();
                    this.ShowEffects(effect);
                }
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

            protected override bool CastAvailable(Avatar avatar) => false;

            public override AbilityPosition AbilityPosition { get; }

            public override AbilityActionAttribute ActionType => AbilityActionAttribute.Special;

            public override AbilityTargetType TargetType => AbilityTargetType.TargetAndNonTarget;
        }

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.Click,
            ControlEventType.GlobalClick,
            ControlEventType.Focus,
            ControlEventType.GlobalClickRelease
        };

        public bool SafeMode { get; set; } = false;
    }
}