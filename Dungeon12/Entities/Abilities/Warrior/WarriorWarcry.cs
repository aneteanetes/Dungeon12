using Dungeon12.Entities.Enums;

namespace Dungeon12.Entities.Abilities.Warrior
{
    public class WarriorWarcry : Ability
    {
        public override Archetype Class => Archetype.Warrior;

        public override void Bind()
        {
            Name = Global.Strings.WarriorWarcry;
            Description = Global.Strings.WarriorWarcryDescT1;
            Area = new AbilityArea(all: true);
            Element = Element.Mental;
            Cooldown = 10;
            UseRange = AbilRange.Any;
        }

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings.Damage}: {Value}",
                $"{Global.Strings.Type}: {Element.Display()}",
                $"{Global.Strings.Range}: {UseRange.Display()}",
                Global.Strings.Taunt
            };
        }
    }
}