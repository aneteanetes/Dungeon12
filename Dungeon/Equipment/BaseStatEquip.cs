namespace Dungeon
{
    using Dungeon.Classes;
    using Dungeon.Items;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BaseStatEquip : Equipment
    {
        public string StatName { get; set; }

        public List<string> StatProperties { get; set; } = new List<string>();

        public StatValues StatValues { get; set; } = new List<long>();

        public override string Title => $"{StatName}: {StatValues.ToString()}";

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

        public virtual bool CanApply(Character character) => true;

        public virtual void CallCanApply(dynamic obj) => this.CanApply(obj);
    }

    public class StatValues
    {
        public Func<List<long>> Provider { get; set; }

        public List<long> Values => Provider();

        public long this[int index]
        {
            get => Values[index];
            set => Values[index] = value;
        }

        public static StatValues Function(Func<IEnumerable<long>> provider) => new StatValues() { Provider = () => provider().ToList() };

        public static implicit operator StatValues(List<long> values)
        {
            return new StatValues()
            {
                Provider = () => values
            };
        }

        public string Template { get; set; }

        public override string ToString()
        {
            if (Template == null)
            {
                Template = string.Join(" ", Enumerable.Range(0, Values.Count()).Select((x, i) => $"{{{i}}}").ToArray());
            }

            return string.Format(Template, Values.Cast<object>().ToArray());
        }

        public static implicit operator string(StatValues statValues)=>statValues.ToString();
    }
}