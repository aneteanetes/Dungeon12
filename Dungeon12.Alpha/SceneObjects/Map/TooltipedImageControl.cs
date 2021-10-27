using System;
using System.Collections.Generic;
using System.Text;
using Dungeon.Control;
using Dungeon.GameObjects;
using Dungeon.View.Interfaces;

namespace Dungeon12.Drawing.SceneObjects.Map
{
    public class TooltipedImageControl : TooltipedSceneObject<EmptyGameComponent>
    {
        protected override ControlEventType[] Handles => new ControlEventType[] { ControlEventType.Focus };

        public TooltipedImageControl(string img, string tooltip) : base(EmptyGameComponent.Empty, tooltip)
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
