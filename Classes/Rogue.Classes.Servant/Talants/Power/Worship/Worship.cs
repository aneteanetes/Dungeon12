using Rogue.Abilities.Talants;

namespace Rogue.Classes.Servant.Talants.Power
{
    public class Worship : Talant<Servant>
    {
        public override bool Activatable => true;

        public override string Group => PowerTalants.Warrior;

        public Worship(int order) : base(order)
        {
        }

        public override string Name => "Служение";//"Молебен";

        public override int MaxLevel => 1;

        public override string Description => $"Освящение теперь всегда пассивно генерирует силу веры";
        
        public override string[] DependsOn => new string[]
        {
            nameof(Overcoming)
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
