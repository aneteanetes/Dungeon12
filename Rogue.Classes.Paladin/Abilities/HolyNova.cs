namespace Rogue.Classes.Paladin.Abilities
{
    using System;
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Classes.Paladin.Talants;

    public class HolyNova : Ability<Paladin, HolyPowers>
    {
        public override int Position => 2;

        public override string Name => "Столп света";

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