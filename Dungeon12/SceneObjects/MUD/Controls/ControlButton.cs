using Dungeon;
using Dungeon.Control;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;

namespace Dungeon12.SceneObjects.MUD.Controls
{
    internal class ControlButton : EmptySceneControl, ITooltipedDrawText
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        private char _char;
        private bool _isdisabled;

        public ControlButton(char @char, string tooltipText, Action click, bool isdisabled = false)
        {
            _isdisabled = isdisabled;
            _char = @char;

            Height = 75;
            Width = 75;
            OnClick = click;
            Image = $"UI/layout/btns/{@char}{(isdisabled ? "d" : "")}.png".AsmImg();
            TooltipText = Global.Strings[tooltipText].AsDrawText().Gabriela();
        }

        private readonly Action OnClick;

        public IDrawText TooltipText { get; set; }

        public bool ShowTooltip => true;

        public override void Focus()
        {
            if (_isdisabled)
                return;

            Image = $"UI/layout/btns/{_char}a.png".AsmImg();
            base.Focus();
        }

        public override void Unfocus()
        {
            if (_isdisabled)
                return;

            Image = $"UI/layout/btns/{_char}.png".AsmImg();
            base.Unfocus();
        }

        public override void Click(PointerArgs args)
        {
            if (_isdisabled)
                return;

            OnClick?.Invoke();
            base.Click(args);
        }
    }
}
