namespace Dungeon12.Drawing.SceneObjects.Map
{
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon12.Abilities;
    using Dungeon12.Abilities.Enums;
    using Dungeon12.Entities;
    using Dungeon12.Entities.Fractions;
    using Dungeon12.Map;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ClickActionSceneObject<T> : TooltipedSceneObject<T>
        where T : Dungeon.Physics.PhysicalObject
    {
        protected readonly PlayerSceneObject playerSceneObject;
        protected readonly T @object;

        public ClickActionSceneObject(PlayerSceneObject playerSceneObject, T @object, string tooltip, bool bindView = true) : base(@object,tooltip, bindView)
        {
            this.@object = @object;

            if (this.GetType() != typeof(PlayerSceneObject))
            {
                if (playerSceneObject != null)
                {
                    this.playerSceneObject = playerSceneObject;
                    this.playerSceneObject.OnMove += CheckStopAction;
                }
            }

            if (this is PlayerSceneObject playerSceneObject1)
            {
                this.playerSceneObject = playerSceneObject1;
            }
        }

        protected virtual int GrowSize { get; set; } = 3;

        protected virtual bool SilentTooltip => false;

        protected virtual Key AlternativeTooltipKey { get; set; } = Key.None;

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.Focus,
            ControlEventType.Click,
            ControlEventType.Key
        };

        protected override Key[] KeyHandles => new Key[]
        {
            Key.Q,
            Key.E,
            Key.LeftShift,
            Key.LeftControl,
            AlternativeTooltipKey
        };

        private bool acting = false;

        /// <summary>
        /// Проблема наследования
        /// </summary>
        public virtual bool Clickable => true;

        protected virtual void BeforeClick() { }

        public override void Click(PointerArgs args)
        {
            BeforeClick();
            if (!Clickable)
                return;

            TryAddFraction();
            SetAbility(args.MouseButton);

            if (CheckActionAvailable(args.MouseButton))
            {
                Global.Interacting = true;

                acting = true;
                Action(args.MouseButton);
            }
            else
            {
                Global.GameState.Player.BindMovePointAction(this.@object, () => Action(args.MouseButton));
            }
        }

        /// <summary>
        /// При взаимодействии с объектом по клику если фракции нет - она добавляется
        /// </summary>
        private void TryAddFraction()
        {
            if (this.Component != default && (this.Component is EntityFraction entityComponent))
            {
                if (entityComponent.Fraction != default && entityComponent.Fraction.Playable)
                {
                    if (!Global.GameState.Character.Fractions.Any(x => x.IdentifyName == entityComponent.Fraction.IdentifyName))
                    {
                        Global.GameState.Character.Fractions.Add(FractionView.Load(entityComponent.Fraction.IdentifyName).ToFraction());
                    }
                }
            }
        }

        protected Ability ability;

        private readonly Dictionary<MouseButton, AbilityPosition> mouseAbiityMap = new Dictionary<MouseButton, AbilityPosition>() { { MouseButton.Left, AbilityPosition.Left }, { MouseButton.Right, AbilityPosition.Right } };
        private void SetAbility(MouseButton mouseButton)
        {
            if (mouseButton == MouseButton.Left || mouseButton == MouseButton.Right)
            {
                ability = playerSceneObject?.GetAbility(mouseAbiityMap[mouseButton]);
            }
        }

        private void CheckStopAction()
        {
            if (!acting)
                return;

            if (!CheckActionAvailable(MouseButton.None))
            {
                StopAction();
                acting = false;
            }
        }

        protected virtual bool CheckActionAvailable(MouseButton mouseButton)
        {
            var range = @object.Grow(3);
            return playerSceneObject.Avatar.IntersectsWith(range);
        }

        protected abstract void Action(MouseButton mouseButton);

        protected virtual void StopAction() { }


        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.LeftShift && !hold)
            {
                if (!SilentTooltip)
                {
                    this.ShowTooltip();
                    DisableTooltipAction = true;
                }
            }

            if (key == AlternativeTooltipKey && !hold)
            {
                this.ShowTooltip();
                DisableTooltipAction = true;
            }

            if (key == Key.Q || key == Key.E)
            {
                this.SetAbility(key);
            }
        }

        private void SetAbility(Key key) => ability = playerSceneObject?.GetAbility(keyAbiityMap[key]);

        private readonly Dictionary<Key, AbilityPosition> keyAbiityMap = new Dictionary<Key, AbilityPosition>() { { Key.Q, AbilityPosition.Q }, { Key.E, AbilityPosition.E } };

        public override void KeyUp(Key key, KeyModifiers modifier)
        {
            if (key == Key.LeftShift)
            {
                DisableTooltipAction = false;
                this.HideTooltip();
            }

            if (key == AlternativeTooltipKey)
            {
                DisableTooltipAction = false;
                this.HideTooltip();
            }
        }

        public override void Focus()
        {
            if (@object is MapObject mapObject)
            {
                if (playerSceneObject != null)
                {
                    playerSceneObject.TargetsInFocus.Add(mapObject);
                }
            }
            base.Focus();
        }

        public override void Unfocus()
        {
            if (@object is MapObject mapObject)
            {
                if (playerSceneObject != null)
                {
                    playerSceneObject.TargetsInFocus.Remove(mapObject);
                }
            }
            base.Unfocus();
        }
    }
}