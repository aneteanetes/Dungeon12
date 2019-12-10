namespace Dungeon12.Drawing.SceneObjects.Map
{
    using Dungeon.Drawing;
    using Dungeon.GameObjects;
    using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using System.Collections.Generic;

    public class EmptyTooltipedSceneObject : TooltipedSceneObject<EmptyGameComponent>
    {
        public EmptyTooltipedSceneObject(string tooltip) : base(EmptyGameComponent.Empty, tooltip)
        {
        }
    }

    public abstract class TooltipedSceneObject<TComponent> : Dungeon12.SceneObjects.HandleSceneControl<TComponent>
        where TComponent : class, IGameComponent
    {
        protected Tooltip aliveTooltip = null;

        public override bool Filtered => false;

        public IDrawColor TooltipTextColor { get; set; }

        public TooltipedSceneObject(TComponent component, string tooltip, bool bindView=true):base(component,bindView)
        {
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
                this.ShowInScene(new List<ISceneObject>() { aliveTooltip });
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
    }
}