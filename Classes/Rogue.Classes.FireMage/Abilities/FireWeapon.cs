namespace Rogue.Classes.FireMage
{
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Classes.FireMage.Talants;

    public class FireWeapon : Ability<FireMage, FireMagic>
    {
        public override int Position => 3;

        public override string Name => "Оружие огня";

        public override ScaleRate Scale => ScaleRate.Build(Rogue.Entites.Enums.Scale.AbilityPower, 0.1);

        public override bool CastAvailable(FireMage @class, FireMagic talants)
        {
            throw new System.NotImplementedException();
        }

        protected override void InternalCast(FireMage @class, FireMagic talants)
        {
            throw new System.NotImplementedException();
        }
    }
}