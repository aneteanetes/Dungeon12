namespace Dungeon.Network
{
    public class NetworkMessage
    {
        public string Recipient { get; set; }

        public string DataType { get; set; }

        public object Data { get; set; }
    }
}
