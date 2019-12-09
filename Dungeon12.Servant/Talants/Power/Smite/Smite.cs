using Dungeon12.Abilities.Talants;

namespace Dungeon12.Servant.Talants.Power
{
    public class Smite : Talant<Servant>
    {
        public Smite(int order) : base(order)
        {
        }

        public override string Name => "Кара";//"Молебен";

        public override string Description => $"Заменяет лечение на нанесение урона";

        public override int MaxLevel => 2;

        public override string[] DependsOn => new string[]
        {
            nameof(Prevails)
        };

        public override int Tier => 2;

        protected override void CallApply(dynamic obj)
        {
            return;
        }

        protected override bool CallCanUse(dynamic obj)
        {
            return true;
        }

        protected override void CallDiscard(dynamic obj)
        {
            return;
        }

        protected override TalantInfo CallTalantInfo(dynamic obj)
        {
            return new TalantInfo();
        }
    }
}
