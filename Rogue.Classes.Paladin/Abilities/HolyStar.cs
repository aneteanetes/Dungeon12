namespace Rogue.Classes.Paladin.Abilities
{
    using System;
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Classes.Paladin.Talants;

    public class HolyStar : Ability<Paladin, HolyPowers>
    {
        public override int Position => 4;

        public override string Name => "Удар светом";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AbilityPower, 0.1);

        public override bool CastAvailable(Paladin @class, HolyPowers talants)
        {
            throw new NotImplementedException();
        }

        protected override void InternalCast(Paladin @class, HolyPowers talants)
        {
            throw new NotImplementedException();
        }
    }
}