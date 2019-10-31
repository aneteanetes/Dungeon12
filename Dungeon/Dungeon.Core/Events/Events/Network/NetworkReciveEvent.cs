namespace Dungeon.Events.Network
{
    public class NetworkReciveEvent : IEvent
    {
        public string Sender { get; set; }

        public object Message { get; set; }
    }
}
