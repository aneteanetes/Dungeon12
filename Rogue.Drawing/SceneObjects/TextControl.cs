namespace Rogue.Drawing.SceneObjects
{
    using Rogue.View.Interfaces;

    public class TextControl : SceneControl
    {
        public TextControl(IDrawText text)
        {
            Text = text;
        }
    }
}
