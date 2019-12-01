using Dungeon.Data.Fractions;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon.Entities.Fractions
{
    public class FractionView : DataEntity<FractionView, FractionData>
    {
        public string[] EnemiesIdentities { get; set; }
                
        protected override void Init(FractionData dataClass)
        {
            Name = dataClass.Name;
            EnemiesIdentities = dataClass.Enemies;
            this.IdentifyName = dataClass.IdentifyName;
        }

        public Fraction ToFraction() => new Fraction(Name)
        {
            Name = Name,
            EnemiesIdentities = EnemiesIdentities
        };
    }

    public class Fraction : FractionView
    {
        public Fraction(string name)
        {
            Progress = new FractionProgress(name);
        }

        public FractionProgress Progress { get; set; }

        public IEnumerable<FractionView> Enemies => Data.Database.EntityQuery<FractionView>(x => EnemiesIdentities.Contains(x.IdentifyName));
    }
}