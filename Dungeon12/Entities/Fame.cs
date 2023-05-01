using Dungeon;
using Dungeon12.Entities.Enums;
using Dungeon12.Entities.Talks;
using Newtonsoft.Json.Linq;

namespace Dungeon12.Entities
{
    internal class Fame
    {
        public Fame() { }

        public Fame(Fraction fraction)
        {
            switch (fraction)
            {
                case Fraction.Vanguard:
                    Good+=10;
                    break;
                case Fraction.MageGuild:
                    Good+=5;
                    Wisdom+=5;
                    break;
                case Fraction.Mercenary:
                    Evil+=5;
                    Trick+=10;
                    break;
                case Fraction.Exarch:
                    Evil+=5;
                    Wisdom+=5;
                    break;
                case Fraction.Cult:
                    break;
                case Fraction.Friendly:
                    break;
                case Fraction.Neutral:
                    break;
                default:
                    break;
            }
        }

        public int Good { get; set; }

        public int Evil { get; set; }

        public int Trick { get; set; }

        public int Wisdom { get; set; }

        public int this[FameType type]
        {
            get
            {
                switch (type)
                {
                    case FameType.Good: return Good;
                    case FameType.Evil: return Evil;
                    case FameType.Trick: return Trick;
                    case FameType.Wisdom: return Wisdom;
                    default: return Common;
                }
            }
        }

        public int Common => Good+Evil+Trick+Wisdom;

        public void Add(int value, FameType type)
        {
            switch (type)
            {
                case Talks.FameType.Good:
                    Good += value;
                    break;
                case Talks.FameType.Evil:
                    Evil += value;
                    break;
                case Talks.FameType.Trick:
                    Trick += value;
                    break;
                case Talks.FameType.Wisdom:
                    Wisdom += value;
                    break;
                default:
                    break;
            }
        }

        public string Name()
        {
            var fames = new int[] { Good, Evil, Trick, Wisdom };
            var max = fames.Max();

            string value = "";

            if (max==Good)
            {
                value += Global.Strings["Fame.Good"];
            }

            if (max==Evil)
            {
                if (value.IsNotEmpty())
                     return Global.Strings["Fame.Mixed"];

                value +=Global.Strings["Fame.Evil"];
            }

            if (max==Trick)
            {
                if (value.IsNotEmpty())
                    return Global.Strings["Fame.Mixed"];

                value +=Global.Strings["Fame.Trick"];
            }

            if (max==Wisdom)
            {
                if (value.IsNotEmpty())
                    return Global.Strings["Fame.Mixed"];

                value +=Global.Strings["Fame.Wisdom"];
            }

            return value;

        }

        public override string ToString()
        {
            return $"Добрые: {Good}; Злые: {Evil}; Мошенники: {Trick}; Мудрецы: {Wisdom};";
        }
    }
}
