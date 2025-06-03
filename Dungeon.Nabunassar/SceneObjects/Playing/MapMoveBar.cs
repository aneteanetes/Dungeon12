using Dungeon.SceneObjects;
using Dungeon.Varying;
using Nabunassar.Game;

namespace Nabunassar.SceneObjects.Playing
{
    internal class MapMoveBar : SceneObject<GameState>
    {
        public MapMoveBar(GameState component) : base(component)
        {
            this.Width = Variables.Get("MapMoveBarWindowW", 400);
            this.Height = Variables.Get("MapMoveBarWindowH", 50);
            this.Left = Variables.Get("MapMoveBarWindowL", 1500);
            this.Top = Variables.Get("MapMoveBarWindowT", -5);

            this.AddBorderBack();
        }
    }
}
