using Nabunassar.Entities;

namespace Nabunassar.Scenes.Creating.Character
{
    internal class CreatePart : SceneControl<Hero>
    {
        public CreatePartCube Cube { get; set; }

        public CreatePart(Hero component) : base(component)
        {
        }
        public bool IsActivated { get; set; }
    }
}
