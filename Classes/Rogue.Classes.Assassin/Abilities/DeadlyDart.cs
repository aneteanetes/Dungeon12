namespace Rogue.Classes.Assassin.Abilities
{
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Classes.Assassin.Talants;

    public class DeadlyDart : Ability<Assassin, Poisons>
    {
        public override int Position => 1;

        public override string Name => "Смертельный дротик";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AttackDamage, 0.1);

        public override bool CastAvailable(Assassin @class, Poisons talants)
        {
            throw new System.NotImplementedException();
        }

        protected override void InternalCast(Assassin @class, Poisons talants)
        {
            throw new System.NotImplementedException();
        }
    }
}