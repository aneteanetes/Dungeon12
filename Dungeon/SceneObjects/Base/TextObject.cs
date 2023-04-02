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

        public void SetText(IDrawText text) => Text = text;

        public override void Throw(Exception ex)
        {
            throw ex;
        }
    }
}
