using Dungeon12.Entities.Enums;

namespace Dungeon12.Entities
{
    internal class SkillInfo
    {
        public string Name { get; set; }

        public bool IsBase { get; set; }

        public bool IsItem { get; set; }

        public int Cooldown { get; set; }

        public string Description { get; set; }
    }
}
