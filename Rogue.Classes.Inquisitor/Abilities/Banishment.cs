namespace Rogue.Classes.Inquisitor.Abilities
{
    using System;
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Classes.Inquisitor.Talants;

    public class Banishment : Ability<Inquisitor, BanishmentTechniques>
    {
        public override int Position => 1;

        public override string Name => "Изгнание";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.Both, 0.1);

        public override bool CastAvailable(Inquisitor @class, BanishmentTechniques talants)
        {
            throw new NotImplementedException();
        }

        protected override void InternalCast(Inquisitor @class, BanishmentTechniques talants)
        {
            throw new NotImplementedException();
        }
    }
}
