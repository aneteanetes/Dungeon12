namespace Rogue.Scenes.Controls
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
