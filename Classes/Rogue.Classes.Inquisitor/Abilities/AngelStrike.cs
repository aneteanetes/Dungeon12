namespace Rogue.Classes.Inquisitor.Abilities
{
    using System;
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Classes.Inquisitor.Talants;

    public class AngelStrike : Ability<Inquisitor, SanctuaryPowers>
    {
        public override int Position => 3;

        public override string Name => "Удар Ангела";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AbilityPower, 0.1);

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
