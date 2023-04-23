using Dungeon12.Entities.Turns;

namespace Dungeon12.SceneObjects.MUD.Turning
{
    internal class TurnPanel : SceneControl<TurnOrder>
    {
        public TurnPanel(TurnOrder component) : base(component)
        {
            Width = 1120;
            Height = 60;

            this.AddBorder();

            AddChildCenter(new TurnsView(component));
        }
    }
}
