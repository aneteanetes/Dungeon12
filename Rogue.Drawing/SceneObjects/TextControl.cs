namespace Rogue.Drawing.SceneObjects
{
    using Rogue.View.Interfaces;

    public class TextControl : SceneObject
    {
        public TextControl(IDrawText text)
        {
            Text = text;
        }
    }
}
