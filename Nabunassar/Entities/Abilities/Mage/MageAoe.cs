using Nabunassar.Entities.Enums;

namespace Nabunassar.Entities.Abilities.Mage
{
    internal class MageAoe : Ability
    {
        public override Archetype Class => Archetype.Mage;

        public override void Bind()
        {
            Area = new AbilityArea(true,true,leftback:true);
            Element = Element.Fire;
            Cooldown = 3;
            UseRange = AbilRange.Any;
        }

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Nabunassar.Global.Strings["Damage"]}: {Value}",
                $"{Global.Strings["Type"]}: {Element.Display()}",
                $"{Global.Strings["Range"]}: {UseRange.Display()}"
            };
        }
    }
}
