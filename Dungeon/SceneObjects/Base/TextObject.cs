namespace Dungeon.SceneObjects
{
    using Dungeon.View.Interfaces;
    using System;

    public class TextObject : SceneObject<IDrawText>
    {
        public TextObject(IDrawText component) : base(component)
        {
            Text = component;

            var m = this.MeasureText(this.Text);

            Width = m.X;
            Height = m.Y;   
        }

        public void SetDrawText(IDrawText text) => Text = text;

        public void SetText(string txt) => Text.SetText(txt);

        public void SetText(IDrawText txt) => Text = txt;

        public override void Throw(Exception ex)
        {
            throw ex;
        }
    }
}
