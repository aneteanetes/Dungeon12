using Dungeon12.Entities.Enums;

namespace Dungeon12.Entities.Abilities.Priest
{
    /// <summary>
    /// Атака оружием, преобразуется в исцеляющую атаку, таунт, оглушение
    /// </summary>
    public class PriestAttack : Ability
    {
        public override Archetype Class => Archetype.Priest;

        public override void Bind()
        {
            Name = Global.Strings.PriestAttack;
            Description = Global.Strings.PriestAttackDescT1;
            Area = new AbilityArea();
            Element = Element.Physical;
            UseRange = AbilRange.Close;
        }

        public int StunChanse { get; set; } = 20;

        public override string[] GetTextParams()
        {
            return new string[] {
                $"{Global.Strings.Damage}: {Value}",
                $"{Global.Strings.Type}: {Element.Display()}",
                $"{Global.Strings.Range}: {UseRange.Display()}",
                $"{Global.Strings.StunChanse}: {StunChanse}%",
            };
        }
    }
}
