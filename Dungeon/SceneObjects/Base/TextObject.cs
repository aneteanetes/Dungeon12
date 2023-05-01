namespace Dungeon.SceneObjects
{
    using Dungeon.View.Interfaces;
    using System;

    public class TextObject : SceneObject<IDrawText>
    {
        public TextObject(IDrawText component) : base(component)
        {
            Text = component;
        }

        public void SetDrawText(IDrawText text) => Text = text;

        public void SetText(string txt) => Text.SetText(txt);

        public override void Throw(Exception ex)
        {
            throw ex;
        }
    }
}
