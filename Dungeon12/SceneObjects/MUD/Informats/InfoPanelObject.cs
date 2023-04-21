using Dungeon12.Entities.Map;

namespace Dungeon12.SceneObjects.MUD.Informats
{
    internal class InfoPanelObject : SceneControl<Polygon>
    {
        public InfoPanelObject(Polygon component) : base(component)
        {
            this.Width=400;
            this.Height= 800;
        }

        public override bool Visible => Component!=default;
    }
}