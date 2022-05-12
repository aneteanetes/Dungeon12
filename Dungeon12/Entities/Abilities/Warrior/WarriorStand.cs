using Dungeon12.Entities.Enums;

namespace Dungeon12.Entities.Abilities.Warrior
{
    public class WarriorStand : Ability
    {
        public override Archetype Class => Archetype.Warrior;

        public override void Bind()
        {
            Area = new AbilityArea(all:true);
            Element = Element.Mental;
            Cooldown = 5;
            UseRange = AbilRange.Any;
        }

        public double DefencePercentage { get; set; } = 20;

        public override int Value { get; set; } = 40;

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings["Defence"]}: +{DefencePercentage}%",
                $"{Global.Strings["Damage"]}: -{Value}%",
            };
        }
    }
}
