using Dungeon12.Entities.Enums;

namespace Dungeon12.Entities.Abilities.Warrior
{
    public class WarriorAttack : Ability
    {
        public override Archetype Class => Archetype.Warrior;

        public override void Bind()
        {
            Name = Global.Strings.WarriorAttack;
            Description = Global.Strings.WarriorAttackDescT1;
            Area = new AbilityArea();
            Element = Element.Physical;
            UseRange = AbilRange.Close;
        }

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings.Damage}: {Value}",
                $"{Global.Strings.Type}: {Element.Display()}",
                $"{Global.Strings.Range}: {UseRange.Display()}",
                Global.Strings.IgnoreTargetPhysicalDefence
            };
        }
    }
}
