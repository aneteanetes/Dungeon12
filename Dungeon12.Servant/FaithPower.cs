using System.Linq;

namespace Dungeon12.Servant
{
    public class FaithPower
    {
        private int _power;
        public int Value
        {
            get => _power;
            set
            {
                if (value > 4)
                    return;

                _power = value;
            }
        }

        public override string ToString()
        {
            return string.Join(" ", Enumerable.Range(0, _power).Select(_ => "*"));
        }
    }
}
