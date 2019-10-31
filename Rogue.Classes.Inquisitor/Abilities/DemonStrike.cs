namespace Rogue.Classes.Inquisitor.Abilities
{
    using System;
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Classes.Inquisitor.Talants;

    public class DemonStrike : Ability<Inquisitor, SanctuaryPowers>
    {
        public override int Position => 4;

        public override string Name => "Удар Демона";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AttackDamage, 0.1);

        public override bool CastAvailable(Inquisitor @class, SanctuaryPowers talants)
        {
            throw new NotImplementedException();
        }

        protected override void InternalCast(Inquisitor @class, SanctuaryPowers talants)
        {
            throw new NotImplementedException();
        }
    }
}