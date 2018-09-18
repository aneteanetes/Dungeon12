namespace Rogue.Classes.BloodMage.Abilities
{
    using System;
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Classes.BloodMage.Talants;

    public class BloodShield : Ability<BloodMage, BloodMutations>
    {
        public override int Position => 3;

        public override string Name => "Щит крови";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AbilityPower, 0.1);

        public override bool CastAvailable(BloodMage @class, BloodMutations talants)
        {
            throw new NotImplementedException();
        }

        protected override void InternalCast(BloodMage @class, BloodMutations talants)
        {
            throw new NotImplementedException();
        }
    }
}