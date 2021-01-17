namespace Dungeon12.Drawing.SceneObjects.Common
{
    using Dungeon;
    using Dungeon12.Abilities;
    using Dungeon12.Abilities.Enums;
    using Dungeon12.Abilities.Scaling;
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon12.Drawing.SceneObjects.Map;
    using Dungeon12.Map;
    using Dungeon12.Map.Objects;
    using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
    using System;
    using System.Collections.Generic;

    public class SkillControl : TooltipedSceneObject<Ability>
    {
        public override bool AbsolutePosition => true;

        private readonly Ability ability;

        private readonly GameMap gameMap;

        private readonly Avatar avatar;

        private readonly PlayerSceneObject player;

        private readonly ImageControl abilControl;

        private readonly AbilityPosition abilityPosition;

        private Action<ISceneObject> destroyBinding;

        private Action<ISceneControl> controlBinding;

        private bool empty = false;

        public SkillControl(GameMap gameMap, PlayerSceneObject player, Ability ability, AbilityPosition abilityPosition, Action<List<ISceneObject>> abilityEffects, Action<ISceneObject> destroyBinding, Action<ISceneControl> controlBinding)
            : base(ability, ability?.Name/*, abilityEffects*/)
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

        private class CooldownMask : Dungeon.Drawing.SceneObjects.ImageControl
        {
            public override bool AbsolutePosition => true;
            public override bool CacheAvailable => false;

            private TextControl text;
            private Ability _ability;

            public CooldownMask(Ability ability) : base(@"ui\square_transparent_mask.png".PathImage())
            {
                _ability = ability;
                this.Mask = Dungeon.Drawing.SceneObjects.ImageMask.Radial()
                    .BindAmount(() => ability.Cooldown?.Percent ?? 0f)
                    .BindVisible(() => ability.Cooldown?.IsActive ?? false);

                this.Mask.Color = new DrawColor(ConsoleColor.Black);
                this.Mask.Opacity = 0.5f;

                text = this.AddTextCenter("0".AsDrawText().InSize(20).Montserrat());
                text.Left+=.7;
                text.Top += 1;
                text.Visible = false;

                Global.DrawClient.CacheObject(this);
            }

            public override void Update()
            {
                if (_ability.Cooldown?.IsActive ?? false)
                {
                    text.Visible=true;
                    var sec = Math.Round(_ability.Cooldown.ElapsedSeconds, 1);
                    text.Text.SetText(sec.ToString().Replace(",","."));
                    if (sec > 5)
                    {
                        text.Text.ForegroundColor = DrawColor.White;
                    }
                    else if (sec < 5 && sec > 2)
                    {
                        text.Text.ForegroundColor = DrawColor.Yellow;
                    }
                    else
                    {
                        text.Text.ForegroundColor = DrawColor.Red;
                    }
                }
                else
                {
                    text.Visible = false;
                }
                base.Update();
            }
        }

        public override double Width => IsBig ? 3 : 2;
        public override double Height => IsBig ? 3 : 2;

        public Action OnClick { get; set; }

        private string SquareTexture(bool focus)
        {
            if (empty || !abilControl.Visible)
                return $"Dungeon12.Resources.Images.ui.square_d.png";

            if (ability != null)
            {
                if (ability.CastType == AbilityCastType.Passive)
                {
                    if (ability.PassiveWorking)
                        return $"Dungeon12.Resources.Images.ui.square_f.png";
                    else
                        return $"Dungeon12.Resources.Images.ui.square_d.png";
                }
            }

            var big = IsBig
                ? "B"
                : "";

            var f = focus
                ? "_f"
                : "";

            return $"Dungeon12.Resources.Images.ui.square{big}{f}.png";
        }

        public bool IsBig => false;
        //this.ability.AbilityPosition == Dungeon12.Abilities.Enums.AbilityPosition.Left
        //    || this.ability.AbilityPosition == Dungeon12.Abilities.Enums.AbilityPosition.Right;

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
                            img = $"Dungeon12.Resources.Images.ui.talk.png";
                            break;
                        case AbilityPosition.Right:
                            this.TooltipText = "Информация";
                            img = $"Dungeon12.Resources.Images.ui.info.png";
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
            if(ability.Hold)
            {
                holds = true;
                highlight = true;
            }

            Cast(null);
        }

        private void Cast(PointerArgs args)
        {
            if (Global.Interacting)
            {
                Global.Interacting = false;
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

            if (player.TargetsInFocus.Count == 0 && (ability.TargetType == AbilityTargetType.NonTarget || ability.TargetType == AbilityTargetType.SelfTarget || ability.TargetType == AbilityTargetType.TargetAndNonTarget))
            {
                if (this.ability.CastAvailableCooldown(avatar))
                {
                    this.ability.CastCooldown(gameMap, avatar);
                    this.highlight = true;
                    this.Image = SquareTexture(true);
                }
            }
        }

        public override void KeyUp(Key key, KeyModifiers modifier)
        {
            if (ability.Hold && holds)
            {
                ability.Release(gameMap, avatar);
                this.Image = SquareTexture(false);
                holds = false;
                highlight = false;
            }
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