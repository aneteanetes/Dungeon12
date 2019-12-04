using Dungeon;
using Dungeon.SceneObjects;
using System;

namespace Dungeon.Entities.Fractions
{
    public class FractionProgress
    {
        public FractionProgress(string fractionName) => FractionName = fractionName;

        public string FractionName { get; set; }

        public int Reputation { get; set; }

        public FractionLevel Level { get; set; }

        public int ReputationMax
        {
            get
            {
                switch (Level)
                {
                    case FractionLevel.Hated:
                    case FractionLevel.Revered:
                        return 10000;

                    case FractionLevel.Hostile:
                    case FractionLevel.Honored:
                        return 1000;

                    case FractionLevel.Unfriendly:
                    case FractionLevel.Friendly:
                        return 100;

                    default:
                        return 0;
                }
            }
        }

    public void Add(int amount)
        {
            Reputation += amount;

            var lvl = Math.Abs(Reputation);
            bool minus = Reputation > 0;

            var wasLevel = Level;

            if (lvl == 0)
            {
                Level = FractionLevel.Neutral;
            }

            if (lvl == 100)
            {
                Level = minus
                    ? FractionLevel.Unfriendly
                    : FractionLevel.Friendly;
            }

            if (lvl == 1000)
            {
                Level = minus
                    ? FractionLevel.Hostile
                    : FractionLevel.Honored;
            }

            if (lvl == 10000)
            {
                Level = minus
                    ? FractionLevel.Hated
                    : FractionLevel.Revered;
            }

            if (wasLevel != Level)
            {
                Toast.Show($"Вы достигли {Level.ToDisplay()} ур. репутации у {FractionName}");
            }
        }
    }
}