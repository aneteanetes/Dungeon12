namespace Dungeon12.Entities
{
    public class SpecInfo
    {
        public string Description { get; set; }

        public string DamageIcon { get; set; }

        public int Damage { get; set; }

        public int Health { get; set; }

        public int Armor { get; set; }

        public string CardName { get; set; }

        public SkillInfo[] Skills { get; set; }

        public bool IsEnabled { get; set; }
    }
}
