namespace Dungeon.SceneObjects
{
    using Dungeon.View.Interfaces;

    public class TextControl : SceneObject<IDrawText>
    {
        public TextControl(IDrawText component) : base(component)
        {
            Text = component;
        }
    }
}
