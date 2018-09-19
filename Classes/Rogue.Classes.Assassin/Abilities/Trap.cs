namespace Rogue.Classes.Assassin.Abilities
{
    using System;
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Classes.Assassin.Talants;

    public class Trap : Ability<Assassin, Assassinity>
    {
        public override int Position => 4;

        public override string Name => "Ловушка";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AttackDamage, 0.1);

        public override bool CastAvailable(Assassin @class, Assassinity talants)
        {
            throw new NotImplementedException();
        }

        protected override void InternalCast(Assassin @class, Assassinity talants)
        {
            throw new NotImplementedException();
        }
    }
}