namespace Rogue.Events.Network
{
    public class NetworkSendEvent : IEvent
    {
        public NetworkSendEvent(object message) => Message = message;

        public string Recipient { get; set; }

        public object Message { get; set; }
    }
}