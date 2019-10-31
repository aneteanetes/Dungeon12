namespace Dungeon.Drawing.SceneObjects.Map
{
    using Dungeon.Abilities;
    using Dungeon.Abilities.Enums;
    using Dungeon.Control.Events;
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon.Map;
    using System.Collections.Generic;

    public abstract class ClickActionSceneObject<T> : TooltipedSceneObject
        where T : Dungeon.Physics.PhysicalObject
    {
        protected readonly PlayerSceneObject playerSceneObject;
        protected readonly T @object;

        public ClickActionSceneObject(PlayerSceneObject playerSceneObject, T @object, string tooltip) : base(tooltip, null)
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
        };

        private bool acting = false;

        /// <summary>
        /// Проблема наследования
        /// </summary>
        public virtual bool Clickable => true;

        public override void Click(PointerArgs args)
        {
            if (!Clickable)
                return;

            SetAbility(args.MouseButton);

            if (CheckActionAvailable(args.MouseButton))
            {
                Global.Interacting = true;

                acting = true;
                Action(args.MouseButton);
            }
        }

        protected Ability ability;

        private readonly Dictionary<MouseButton, AbilityPosition> mouseAbiityMap = new Dictionary<MouseButton, AbilityPosition>() { { MouseButton.Left, AbilityPosition.Left }, { MouseButton.Right, AbilityPosition.Right } };
        private void SetAbility(MouseButton mouseButton) => ability = playerSceneObject.GetAbility(mouseAbiityMap[mouseButton]);

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
                this.ShowTooltip();
            }

            if (key == Key.Q || key == Key.E)
            {
                this.SetAbility(key);
            }
        }

        private void SetAbility(Key key) => ability = playerSceneObject.GetAbility(keyAbiityMap[key]);

        private readonly Dictionary<Key, AbilityPosition> keyAbiityMap = new Dictionary<Key, AbilityPosition>() { { Key.Q, AbilityPosition.Q }, { Key.E, AbilityPosition.E } };

        public override void KeyUp(Key key, KeyModifiers modifier)
        {
            if (key == Key.LeftShift || key == Key.RightShift)
            {
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