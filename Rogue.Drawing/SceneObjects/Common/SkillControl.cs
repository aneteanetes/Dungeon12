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
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Drawing.SceneObjects.Dialogs.NPC;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.View.Interfaces;

    public class SkillControl : TooltipedSceneObject
    {
        private bool safeMode = false;

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
            :base(ability?.Name,abilityEffects)
        {
            this.controlBinding = controlBinding;
            this.destroyBinding = destroyBinding;
            this.abilityPosition = abilityPosition;
            this.avatar = player.Avatar;
            this.player = player;
            this.gameMap = gameMap;
            this.ability = ability;

            if (ability == null)
            {
                empty = true;
                this.ability = new EmptyAbility(abilityPosition);
            }

            var a = this.ability;

            abilControl = new ImageControl(IsBig ? a.Image_B : a.Image)
            {
                CacheAvailable=false
            };
            this.AddChild(abilControl);

            this.Image = SquareTexture(false);
        }

        public override double Width => IsBig ? 3 : 2;
        public override double Height => IsBig ? 3 : 2;

        public Action OnClick { get; set; }

        private string SquareTexture(bool focus)
        {
            if(empty)
                return $"Rogue.Resources.Images.ui.square_d.png";

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
                    safeMode = true;
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
                            this.TooltipText = ability.Name;
                            abilControl.Visible = false;
                            break;
                    }

                    abilControl.Image = img;
                }
                else
                {
                    safeMode = false;
                    this.TooltipText = ability.Name;
                    abilControl.Visible = true;
                    abilControl.Image = this.ability.Image;
                }

                return image;
            }
            set => image = value;
        }
        
        private bool holds = false;

        private void GlobalSafeClick(PointerArgs args)
        {
            var conversational = gameMap.Conversations(this.avatar).FirstOrDefault();

            if (conversational != null)
            {
                player.StopMovings();
                ShowEffects?.Invoke(new List<ISceneObject>
                {
                    new NPCDialogue(conversational,destroyBinding,controlBinding)
                });
            }
        }

        private bool InteractInterrupt()
        {
            var interactable = gameMap.Interactions(this.avatar).FirstOrDefault();

            if (interactable != null)
            {
                interactable.Interact(interactable);
                return true;
            }

            return false;
        }

        public override void GlobalClick(PointerArgs args)
        {
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
                var t = new System.Timers.Timer(200);
                t.AutoReset = false;
                t.Elapsed += (x, y) => this.Image = SquareTexture(false);
                t.Start();
            }
            else
            {
                holds = true;
            }
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
            }
        }

        public override void Focus()
        {
            if (!SafeZoneInvisible)
                this.Image = SquareTexture(true);

            base.Focus();
        }

        public override void Unfocus()
        {
            if (!SafeZoneInvisible)
                this.Image = SquareTexture(false);

            base.Unfocus();
        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            Cast(null);
        }

        private void Cast(PointerArgs args)
        {
            if (SafeZoneInvisible)
            {
                GlobalSafeClick(args);
                this.Image = SquareTexture(true);
                return;
            }

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
            ControlEventType.Focus,
            ControlEventType.GlobalClickRelease
        };
    }
}