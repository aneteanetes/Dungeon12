namespace Rogue.Classes.BloodMage.Abilities
{
    using System;
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Classes.BloodMage.Talants;

    public class BloodSpear : Ability<BloodMage, BloodStream>
    {
        public override int Position => 2;

        public override string Name => "Копьё крови";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AbilityPower, 0.1);

        public override bool CastAvailable(BloodMage @class, BloodStream talants)
        {
            throw new NotImplementedException();
        }

        protected override void InternalCast(BloodMage @class, BloodStream talants)
        {
            throw new NotImplementedException();
        }
    }
}
