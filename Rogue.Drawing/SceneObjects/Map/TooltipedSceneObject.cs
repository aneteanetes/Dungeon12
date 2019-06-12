namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Drawing.GUI;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Entites.Alive;
    using Rogue.Entites.Animations;
    using Rogue.Entites.Enemy;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;

    public abstract class TooltipedSceneObject : HandleSceneControl
    {
        protected Tooltip aliveTooltip = null;
        
        public TooltipedSceneObject(string tooltip, Action<List<ISceneObject>> showEffects)
        {
            if (showEffects != null)
            {
                this.ShowEffects = showEffects;
            }
            TooltipText = tooltip;
        }

        protected string TooltipText;

        public override void Focus()
        {
            if (!string.IsNullOrEmpty(TooltipText))
            {
                aliveTooltip = new Tooltip(TooltipText, new Point(this.ComputedPosition.X, this.ComputedPosition.Y - 0.8))
                {
                    CacheAvailable = false
                };

                this.ShowEffects(new List<ISceneObject>() { aliveTooltip });
            }
        }

        public override void Unfocus()
        {
            aliveTooltip?.Destroy?.Invoke();
            aliveTooltip = null;
        }
    }
}