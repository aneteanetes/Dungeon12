using Dungeon.Entities;
using Dungeon12.Database.Fractions;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Entities.Fractions
{
    public class FractionView : DataEntity<FractionView, FractionData>
    {
        public string[] EnemiesIdentities { get; set; }

        protected override void Init(FractionData dataClass)
        {
            this.Name = dataClass.Name;
            EnemiesIdentities = dataClass.Enemies;
        }

        public Fraction ToFraction() => new Fraction(this.Name)
        {
            Name = this.Name,
            EnemiesIdentities = this.EnemiesIdentities
        };
    }

    public class Fraction : FractionView
    {
        public Fraction(string name)
        {
            Progress = new FractionProgress(name);
        }

        public FractionProgress Progress { get; set; }

        public IEnumerable<FractionView> Enemies => Dungeon.Data.Database.EntityQuery<FractionView>(x => EnemiesIdentities.Contains(x.IdentifyName));
    }
}