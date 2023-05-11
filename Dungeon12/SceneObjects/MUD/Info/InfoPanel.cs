using Dungeon.Control;
using Dungeon.SceneObjects.Grouping;
using Dungeon12.SceneObjects.MUD.Locations;

namespace Dungeon12.SceneObjects.MUD.Info
{
    internal class InfoPanel : SceneControl<Game>
    {
        CharacterInfo character;

        public InfoPanel(Game component) : base(component)
        {
            Width = 400;
            Height = 800;

            this.AddBorder();

            character = this.AddChild(new CharacterInfo());
        }

        public override void Click(PointerArgs args)
        {
        }
    }
}
