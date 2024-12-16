using Nabunassar.Entities.Enums;

namespace Nabunassar.Entities.Abilities.Warrior
{
    internal class WarriorThrow : Ability
    {
        public override Archetype Archetype => Archetype.Warrior;

        public override void Bind()
        {
            Area = new AbilityArea();
            Element = Element.Physical;
            Cooldown = 3;
            UseRange = AbilRange.Far;
        }

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings["Damage"]}: {Value}",
                $"{Global.Strings["Type"]}: {Element.Display()}",
                Global.Strings["Taunt"],
                $"{Global.Strings["Range"]}: {UseRange.Display()}",
            };
        }
    }
}
