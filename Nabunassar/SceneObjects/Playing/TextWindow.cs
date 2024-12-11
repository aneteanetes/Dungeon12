using Dungeon.SceneObjects;
using Dungeon.Varying;
using Nabunassar.Game;

namespace Nabunassar.SceneObjects.Playing
{
    internal class TextWindow : SceneObject<GameState>
    {
        public TextWindow(GameState component) : base(component)
        {
            this.Width = Variables.Get("TextWindowW", 900);
            this.Height = Variables.Get("TextWindowH", 250);
            this.Left = Variables.Get("TextWindowL", 450);
            this.Top = Variables.Get("TextWindowT", 830); ;

            this.AddBorderBack();
        }
    }
}
