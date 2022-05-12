using Dungeon.Control;
using Dungeon.SceneObjects;
using Dungeon12.ECS.Components;
using System;

namespace Dungeon12.SceneObjects.Stats
{
    internal class SlideButton : EmptySceneControl, ITooltiped
    {
        private string img;
        Action _click;

        public SlideButton(Action click, bool left = true)
        {
            img = left ? "left" : "right";
            this.Width=50;
            this.Height=50;
            _click = click;

            this.Image=$"UI/Windows/Stats/{img}.png";
        }

        public string TooltipText { get; set; }

        public override void Click(PointerArgs args)
        {
            _click?.Invoke();
            base.Click(args);
        }
    }
}