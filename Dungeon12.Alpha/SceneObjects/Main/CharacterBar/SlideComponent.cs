using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon.View.Interfaces;
using System;using Dungeon;using Dungeon.Drawing.SceneObjects;
using System.Collections.Generic;
using System.Text;
using Dungeon.GameObjects;

namespace Dungeon12.Drawing.SceneObjects.Main.CharacterBar
{
    public abstract class SlideComponent : TooltipedSceneObject<EmptyGameComponent>
    {
        public virtual double SlideOffsetLeft { get; set; } = 0;
        public virtual double SlideOffsetTop { get; set; } = 0;

        public Func<bool> SlideNeed = () => true;

        public SlideComponent(string tooltip="") : base(EmptyGameComponent.Empty,tooltip)
        {
        }

        public double LeftOriginal => base.Left;

        public override double Left
        {
            get => SlideNeed.Invoke() ? base.Left+SlideOffsetLeft : base.Left;
            set => base.Left = value;
        }

        public double TopOriginal => base.Top;

        public override double Top
        {
            get => SlideNeed.Invoke() ? base.Top + SlideOffsetTop : base.Top;
            set => base.Top = value;
        }
    }
}