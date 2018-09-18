namespace Rogue.Classes.Inquisitor.Abilities
{
    using System;
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Classes.Inquisitor.Talants;

    public class SummonAlly : Ability<Inquisitor, SummonTechniques>
    {
        public override int Position => 2;

        public override string Name => "Призыв союзника";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.Both, 0.1);

        public override bool CastAvailable(Inquisitor @class, SummonTechniques talants)
        {
            throw new NotImplementedException();
        }

        protected override void InternalCast(Inquisitor @class, SummonTechniques talants)
        {
            throw new NotImplementedException();
        }
    }
}
