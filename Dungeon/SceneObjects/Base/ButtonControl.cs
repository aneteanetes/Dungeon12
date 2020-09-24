using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.Utils;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.SceneObjects.Base
{
    [Hidden]
    public class ButtonControl<TComponent> : ControlSceneObject<TComponent>
        where TComponent : class, IGameComponent
    {
        protected TextControl textControl;

        public ButtonControl(TComponent component, string text, float size = 30) : this(component, new DrawText(text, ConsoleColor.White) { Size = size }.DefaultFont()) { }

        public ButtonControl(TComponent component, IDrawText text, float size = 30) : base(component)
        {
            text.Size = size;
            textControl = new TextControl(text);

            var measure = DungeonGlobal.DrawClient.MeasureText(textControl.Text);

            var width = this.Width * Settings.DrawingSize.CellF;
            var height = this.Height * Settings.DrawingSize.CellF;

            var left = width / 2 - measure.X / 2;
            var top = height / 2 - measure.Y / 2;

            //left /= 1.8f;

            textControl.Left = left / Settings.DrawingSize.CellF;
            textControl.Top = top / Settings.DrawingSize.CellF;

            this.Children.Add(textControl);
        }

        public void SetText(string txt)
        {
            this.textControl.Text.SetText(txt);
        }

        public Action OnClick { get; set; }

        public override void Click(PointerArgs args)
        {
            OnClick?.Invoke();
        }
    }
}