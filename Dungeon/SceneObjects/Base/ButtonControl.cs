﻿using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.Utils;
using Dungeon.View.Interfaces;
using System;

namespace Dungeon.SceneObjects.Base
{
    [Hidden]
    public class ButtonControl<TComponent> : SceneControl<TComponent>
        where TComponent : class
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        protected TextObject textControl;

        public ButtonControl(TComponent component, string text, float size = 30) : this(component, new DrawText(text, ConsoleColor.White) { Size = size }.DefaultFont()) { }

        public ButtonControl(TComponent component, IDrawText text, float size = 30) : base(component)
        {
            text.Size = size;
            textControl = new TextObject(text);

            var measure = this.MeasureText(textControl.Text);

            var width = this.Width * Settings.DrawingSize.CellF;
            var height = this.Height * Settings.DrawingSize.CellF;

            var left = width / 2 - measure.X / 2;
            var top = height / 2 - measure.Y / 2;

            //left /= 1.8f;

            textControl.Left = left / Settings.DrawingSize.CellF;
            textControl.Top = top / Settings.DrawingSize.CellF;

            this.AddChild(textControl);
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