namespace Rogue.Classes.Assassin.Abilities
{
    using System;
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Classes.Assassin.Talants;

    public class WeakPoison : Ability<Assassin, Poisons>
    {
        public override int Position => 3;

        public override string Name => "Ослабляющий яд";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AttackDamage, 0.1);

        public override bool CastAvailable(Assassin @class, Poisons talants)
        {
            throw new NotImplementedException();
        }

        protected override void InternalCast(Assassin @class, Poisons talants)
        {
            throw new NotImplementedException();
        }
    }
}