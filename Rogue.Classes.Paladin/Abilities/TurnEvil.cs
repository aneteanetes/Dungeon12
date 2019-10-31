namespace Rogue.Classes.Paladin.Abilities
{
    using System;
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Classes.Paladin.Talants;

    public class TurnEvil : Ability<Paladin, HolyPowers>
    {
        public override int Position => 3;

        public override string Name => "Изгнание зла";

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