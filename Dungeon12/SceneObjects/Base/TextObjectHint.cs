using Dungeon;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Enums;
using System;

namespace Dungeon12.SceneObjects.Base
{
    internal class TextObjectHint : SceneControl<IDrawText>, ITooltiped, IMouseHint, ICursored
    {
        public TextObjectHint(IDrawText component, string tooltip = "", string title = "", string text = "", params string[] leftparams) : base(component)
        {
            var m = this.MeasureText(component);
            Width=m.X;
            Height=m.Y;

            Text = component;
            TooltipText = tooltip;
            GetHint = () =>
            {
                GameHint _hint = null;
                if (title.IsNotEmpty())
                {
                    _hint=new GameHint(title, text, opacity: 1, leftparams: leftparams);
                }

                return _hint;
            };
        }

        private Func<GameHint> GetHint { get; set; }

        public string TooltipText { get; set; }

        public CursorImage Cursor => CursorImage.Question;

        public GameHint CreateMouseHint() => GetHint();

        public void SetText(IDrawText text) => Text = text;
    }
}
