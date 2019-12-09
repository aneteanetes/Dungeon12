namespace Dungeon12
{
    using Dungeon;
    using Dungeon12.Classes;
    using Dungeon12.Items;
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
        protected override void CallDiscard(dynamic obj) => this.Discard(obj);

        public virtual bool CanApply(Character character) => true;

        public virtual void CallCanApply(dynamic obj) => this.CanApply(obj);
    }

    public class StatValues
    {
        /// <summary>
        /// Сериализация... использовать аксессор!
        /// </summary>
        public List<long> Values { get; set; } = new List<long>();

        public long this[int index] => Values.ElementAtOrDefault(index);

        public string Template { get; set; }

        public override string ToString()
        {
            if (Template == null)
            {
                Template = string.Join(" ", Enumerable.Range(0, Values.Count()).Select((x, i) => $"{{{i}}}").ToArray());
            }

            return string.Format(Template, Values.Cast<object>().ToArray());
        }

        public static implicit operator StatValues(List<long> values) => new StatValues() { Values = values };
        public static implicit operator string(StatValues statValues) => statValues.ToString();
    }
}