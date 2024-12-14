namespace Nabunassar.Entities.Abilities.Battle
{
    internal class BattleAbilities : Quad<BattleAbility>
    {
        public BattleAbility Base { get => base.First; set => base.First = value; }

        public BattleAbility Primary { get => base.Second; set => base.Second = value; }

        public BattleAbility Secondary { get => base.Third; set => base.Third = value; }

        public BattleAbility Ultimate { get => base.Fourth; set => base.Fourth = value; }
    }
}
