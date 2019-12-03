using Dungeon.Data.Fractions;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon.Entities.Fractions
{
    public class FractionView : DataEntity<FractionView, FractionData>
    {
        public string[] EnemiesIdentities { get; set; }

        public bool Playable { get; set; }

        public FractionLevel DefaultLevel { get; set; }

        public int DefaultValue { get; set; }

        protected override void Init(FractionData dataClass)
        {
            Name = dataClass.Name;
            EnemiesIdentities = dataClass.Enemies;
            this.IdentifyName = dataClass.IdentifyName;
            this.Playable = dataClass.Playable;
            this.DefaultLevel = dataClass.DefaultLevel;
            this.DefaultValue = dataClass.DefaultValue;
        }

        public Fraction ToFraction() => new Fraction(Name, DefaultLevel, DefaultValue)
        {
            Name = Name,
            IdentifyName=this.IdentifyName,
            EnemiesIdentities = EnemiesIdentities,
            Playable=this.Playable
        };
    }

    public class Fraction : FractionView
    {
        public Fraction(string name, FractionLevel defaultLevel, int defaultValue)
        {
            Progress = new FractionProgress(name);
            Progress.Level = defaultLevel;
            Progress.Reputation = defaultValue;
        }

        public FractionProgress Progress { get; set; }

        public IEnumerable<FractionView> Enemies => Data.Database.EntityQuery<FractionView>(x => EnemiesIdentities.Contains(x.IdentifyName));
    }
}