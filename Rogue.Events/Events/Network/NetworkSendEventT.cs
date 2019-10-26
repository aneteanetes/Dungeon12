namespace Rogue.Events.Network
{
    public class NetworkSendEvent<T> : IEvent
    {
        public NetworkSendEvent(T message) => Message = message;

        public string Recipient { get; set; }

        public T Message { get; set; }
    }
}