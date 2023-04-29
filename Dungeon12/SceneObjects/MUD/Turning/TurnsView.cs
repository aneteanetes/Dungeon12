using Dungeon12.Entities.Turning;

namespace Dungeon12.SceneObjects.MUD.Turning
{
    internal class TurnsView : SceneControl<Turns>
    {
        public TurnsView(Turns component) : base(component)
        {
            Height = 60;
            Width = 45 * component.Count;

            var left = 0d;

            foreach (var turn in component)
            {
                this.AddChild(new TurnIcon(turn)
                {
                    Top=5,
                    Left=left
                });
                left+=33;
            }
        }
    }
}
