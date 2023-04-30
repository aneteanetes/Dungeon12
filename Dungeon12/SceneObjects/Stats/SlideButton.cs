using Dungeon.Control;
using Dungeon12.ECS.Components;
using Dungeon;
using Dungeon.View.Interfaces;

namespace Dungeon12.SceneObjects.Stats
{
    internal class SlideButton : EmptySceneControl, ITooltipedDrawText
    {
        private string img;
        Action _click;

        public SlideButton(string tooltipText, Action click, bool left = true)
        {
            img = left ? "left" : "right";
            Width=50;
            Height=50;
            _click = click;
            Image = $"UI/Windows/Stats/{img}.png";
            TooltipText = Global.Strings[tooltipText].AsDrawText().Gabriela();;
        }

        public IDrawText TooltipText { get; }
        public bool ShowTooltip => true;

        public override void Click(PointerArgs args)
        {
            _click?.Invoke();
            base.Click(args);
        }
    }
}