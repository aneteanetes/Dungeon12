namespace Rogue.Events.Network
{
    public class NetworkReciveEvent<T> : IEvent
    {
        public string Sender { get; set; }

        public T Message { get; set; }
    }
}
