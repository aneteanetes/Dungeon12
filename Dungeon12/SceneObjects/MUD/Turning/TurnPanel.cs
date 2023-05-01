using Dungeon12.Entities.Turning;

namespace Dungeon12.SceneObjects.MUD.Turning
{
    internal class TurnPanel : SceneControl<Turns>
    {
        TurnsView turnsView;

        public TurnPanel(Turns component) : base(component)
        {
            Width = 1120;
            Height = 60;

            this.AddBorder();

            turnsView = AddChildCenter(new TurnsView(component));
            component.BeforeNewRoundTurn += t =>
            {
                turnsView.Destroy();
                turnsView = AddChildCenter(new TurnsView(t));
            };
        }

        public override void Focus()
        {
            base.Focus();
        }
    }
}