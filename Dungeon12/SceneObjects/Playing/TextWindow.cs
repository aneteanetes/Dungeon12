using Dungeon.SceneObjects;
using Dungeon.Varying;

namespace Dungeon12.SceneObjects.Playing
{
    internal class TextWindow : SceneObject<Dungeon12.Game>
    {
        public TextWindow(Dungeon12.Game component) : base(component)
        {
            this.Width = Variables.Get("TextWindowW", 900);
            this.Height = Variables.Get("TextWindowH", 250);
            this.Left = Variables.Get("TextWindowL", 450);
            this.Top = Variables.Get("TextWindowT", 830); ;

            this.AddBorderBack();
        }
    }
}
