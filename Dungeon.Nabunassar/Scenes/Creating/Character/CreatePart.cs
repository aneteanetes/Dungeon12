using Dungeon.Control;
using Nabunassar.Entities;

namespace Nabunassar.Scenes.Creating.Character
{
    internal class CreatePart : SceneControl<Hero>
    {
        public CreatePartCube Cube { get; set; }
        private string _hint;

        public CreatePart(Hero component, string hint) : base(component)
        {
            _hint= hint;
        }

        public bool IsActivated { get; set; }

        protected override ControlEventType[] Handles => [ControlEventType.Focus];

        public override void Focus()
        {
            Global.Game.Creation.Hint = _hint;
            base.Focus();
        }
    }
}
