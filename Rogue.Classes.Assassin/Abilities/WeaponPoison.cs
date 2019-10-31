namespace Rogue.Classes.Assassin.Abilities
{
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Classes.Assassin.Talants;

    public class WeaponPoison : Ability<Assassin, Assassinity>
    {
        public override int Position => 0;

        public override string Name => "Яд на оружии";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AttackDamage, 0.1);

        public override bool CastAvailable(Assassin @class, Assassinity talants)
        {
            throw new System.NotImplementedException();
        }

        protected override void InternalCast(Assassin @class, Assassinity talants)
        {
            throw new System.NotImplementedException();
        }
    }
}