namespace Dungeon12.Struct
{
    internal class Event<T>
    {
        public Event(T value)
        {
            Value= value;
        }

        private T _value;
        public T Value
        {
            get => OnGet(_value);
            set
            {
                _value = value;
                OnSet(_value);
            }
        }

        public Action<T> OnSet { get; set; } = delegate { };

        public Func<T, T> OnGet { get; set; } = x => x;

        public static implicit operator T(Event<T> e) => e.Value;

        public static implicit operator Event<T>(T e) => new(e);
    }
}
