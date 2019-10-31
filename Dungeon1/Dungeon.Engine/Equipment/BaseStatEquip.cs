namespace Rogue
{
    using Dungeon.Classes;
    using Dungeon.Items;
    using System;
    using System.Collections.Generic;

    public class BaseStatEquip : Equipment
    {
        public string StatName { get; set; }

        public List<string> StatProperties { get; set; } = new List<string>();

        public List<long> StatValues { get; set; } = new List<long>();

        public override string Title => $"{StatName}: {string.Join('-', StatValues)}";

        public void Apply(Character character) => Action(character, (a, b) => a + b);

        public void Discard(Character character) => Action(character, (a, b) => a - b);

        private void Action(Character character, Func<long, long, long> operation)
        {
            foreach (var stat in StatProperties)
            {
                var indx = StatProperties.IndexOf(stat);
                var now = character.GetProperty<long>(stat);
                character.SetProperty(stat, operation(now, StatValues[indx]));
            }
        }

        protected override void CallApply(dynamic obj) => this.Apply(obj);
        protected override void CallDiscard(dynamic obj)=> this.Discard(obj);
    }
}