namespace Dungeon.Drawing.SceneObjects.Map
{
    using Dungeon.SceneObjects;
    using Dungeon.Drawing.SceneObjects.UI;
    using Dungeon.Entites.Alive;
    using Dungeon.Entites.Animations;
    using Dungeon.Entites.Enemy;
    using Dungeon.Map;
    using Dungeon.Map.Objects;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;
    using Dungeon.Drawing;

    public abstract class TooltipedSceneObject : HandleSceneControl
    {
        protected Tooltip aliveTooltip = null;

        public override bool Filtered => false;

        public IDrawColor TooltipTextColor { get; set; }

        public TooltipedSceneObject(string tooltip, Action<List<ISceneObject>> showEffects=null)
        {
            if (showEffects != null)
            {
                this.ShowEffects = showEffects;
            }
            TooltipText = tooltip;
        }

        protected string TooltipText;

        protected DrawText TooltipDrawText;

        public override void Focus()
        {
            base.Focus();
            if (!DisableTooltipAction)
            {
                ShowTooltip();
            }
        }

        protected bool DisableTooltipAction = false;

        protected void ShowTooltip()
        {
            if (!string.IsNullOrEmpty(TooltipText) || TooltipDrawText!=null || ProvidesTooltip)
            {
                if (aliveTooltip != null)
                {
                    HideTooltip();
                }

                if (TooltipText != null)
                {
                    TooltipDrawText = new DrawText(TooltipText, TooltipTextColor)
                    {
                        Size = 12,
                        FontName = "Montserrat"
                    };
                }

                var tooltipPosition = new Point(this.ComputedPosition.X, this.ComputedPosition.Y - 0.8);

                aliveTooltip = ProvideTooltip(tooltipPosition) ?? new Tooltip(TooltipDrawText, tooltipPosition);
                aliveTooltip.CacheAvailable = false;
                aliveTooltip.AbsolutePosition = this.AbsolutePosition;
                aliveTooltip.Layer = 100;

                this.Destroy += () => aliveTooltip?.Destroy?.Invoke();
                this.ShowEffects(new List<ISceneObject>() { aliveTooltip });
            }
        }

        protected virtual bool ProvidesTooltip => false;

        protected virtual Tooltip ProvideTooltip(Point position) => null;

        public override void Unfocus()
        {
            base.Unfocus();
            if (!DisableTooltipAction)
            {
                HideTooltip();
            }
        }

        protected void HideTooltip()
        {
            aliveTooltip?.Destroy?.Invoke();
            aliveTooltip = null;
        }
        
        [FlowMethod(typeof(AddEffectContext))]
        public void AddEffect(bool forward)
        {
            if (!forward)
            {
                var effects = this.GetFlowProperty(nameof(AddEffectContext.Effects), Enumerable.Empty<ISceneObject>());
                foreach (var effect in effects)
                {
                    this.AddChild(effect);
                }
            }
        }

        [FlowMethod(typeof(AddEffectContext))]
        public void ShowEffect(bool forward)
        {
            if (!forward)
            {
                var effects = this.GetFlowProperty(nameof(AddEffectContext.Effects), Enumerable.Empty<ISceneObject>());
                this.ShowEffects?.Invoke(effects.ToList());
            }
        }

        public class AddEffectContext
        {
            public IEnumerable<ISceneObject> Effects { get; set; }
        }
    }
}