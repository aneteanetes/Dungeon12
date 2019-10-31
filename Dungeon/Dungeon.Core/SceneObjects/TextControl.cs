namespace Dungeon.Drawing.SceneObjects
{
    using Dungeon.View.Interfaces;

    public class TextControl : SceneObject
    {
        public TextControl(IDrawText text)
        {
            Text = text;
        }
    }
}
