using System.Collections;

namespace Nabunassar.Entities
{
    public class Quad<T> : IEnumerable<T>
    {
        public virtual T First { get; set; }

        public virtual T Second { get; set; }

        public virtual T Third { get; set; }

        public virtual T Fourth { get; set; }

        public virtual IEnumerator<T> GetEnumerator()
        {
            yield return First;
            yield return Second;
            yield return Third;
            yield return Fourth;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public T this[int key]
        {
            get => key switch
            {
                0 => First,
                1 => Second,
                2 => Third,
                3 => Fourth,
                _ => default,
            };

            set
            {
                switch (key)
                {
                    case 0: First = value; break;
                    case 1: Second = value; break;
                    case 2: Third = value; break;
                    case 3: Fourth = value; break;
                    default: throw new InvalidOperationException("Quad support only 0-3 indexes!");
                }
            }
        }
    }
}