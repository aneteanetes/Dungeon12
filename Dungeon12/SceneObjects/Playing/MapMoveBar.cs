using Dungeon.SceneObjects;
using Dungeon.Varying;

namespace Dungeon12.SceneObjects.Playing
{
    internal class MapMoveBar : SceneObject<Dungeon12.Game>
    {
        public MapMoveBar(Dungeon12.Game component) : base(component)
        {
            this.Width = Variables.Get("MapMoveBarWindowW", 460);
            this.Height = Variables.Get("MapMoveBarWindowH", 50);
            this.Left = Variables.Get("MapMoveBarWindowL", 1420);
            this.Top = Variables.Get("MapMoveBarWindowT", -5);

            this.AddBorderBack();
        }
    }
}
