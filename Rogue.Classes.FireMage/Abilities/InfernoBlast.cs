namespace Rogue.Classes.FireMage
{
    using Rogue.Abilities;
    using Rogue.Abilities.Scaling;
    using Rogue.Classes.FireMage.Talants;

    public class InfernoBlast : Ability<FireMage, Infernal>
    {
        public override int Position => 1;

        public override string Name => "Вспышка инферно";

        public override ScaleRate Scale => ScaleRate.Build(Rogue.Entites.Enums.Scale.AbilityPower, 0.1);

        public override bool CastAvailable(FireMage @class, Infernal talants)
        {
            throw new System.NotImplementedException();
        }

        protected override void InternalCast(FireMage @class, Infernal talants)
        {
            throw new System.NotImplementedException();
        }
    }
}