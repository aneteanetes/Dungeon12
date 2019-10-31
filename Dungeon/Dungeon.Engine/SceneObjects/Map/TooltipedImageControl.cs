using System;
using System.Collections.Generic;
using System.Text;
using Dungeon.Control.Events;
using Dungeon.View.Interfaces;

namespace Dungeon.Drawing.SceneObjects.Map
{
    public class TooltipedImageControl : TooltipedSceneObject
    {
        protected override ControlEventType[] Handles => new ControlEventType[] { ControlEventType.Focus };

        public TooltipedImageControl(string img, string tooltip, Action<List<ISceneObject>> showEffects) : base(tooltip, showEffects)
        {
            this.Image = img;
            var measure = this.MeasureImage(img);

            this.Height = measure.Y;
            this.Width = measure.X;
        }

        public override void Focus()
        {
            base.Focus();
        }
    }
}
