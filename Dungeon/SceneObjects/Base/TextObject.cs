namespace Dungeon.SceneObjects
{
    using Dungeon.View.Interfaces;

    public class TextObject : SceneObject<IDrawText>
    {
        public TextObject(IDrawText component) : base(component)
        {
            Text = component;
        }

        public void SetText(IDrawText text) => Text = text;
    }
}
