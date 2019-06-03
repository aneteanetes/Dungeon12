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

    public abstract class TooltipedSceneObject : AnimatedSceneObject
    {
        protected MapObject mapObj;
        protected Tooltip aliveTooltip = null;

        public TooltipedSceneObject(MapObject mapObj, Rectangle defaultFramePosition, Func<int, AnimationMap, bool> requestNextFrame = null) : base(defaultFramePosition, requestNextFrame)
        {
            this.mapObj = mapObj;
        }

        public override void Focus()
        {
            aliveTooltip = new Tooltip(mapObj.Name, new Point(this.Position.X, this.Position.Y - 0.8));

            this.ShowEffects(new List<ISceneObject>() { aliveTooltip });
        }

        public override void Unfocus()
        {
            aliveTooltip.Destroy?.Invoke();
            aliveTooltip = null;
        }
    }
}