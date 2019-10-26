using Lidgren.Network;
using MessagePack;
using Rogue.Events.Network;
using System;

namespace Rogue.Network
{
    internal partial class Network
    {
        private static readonly Network instance = new Network();
        private bool? IsServer = false;
        private NetPeer peer;

        public Network()
        {
            Console.WriteLine(instance);

            Global.Events.Subscribe<CreateNetworkServerEvent>(StartSever);
            Global.Events.Subscribe<ConnectNetworkServerEvent>(StartClient);

            Global.Events.Subscribe<NetworkSendEvent, Network>(Send);
        }

        public NetworkMessage Deserialize(byte[] data)
        {
            return MessagePackSerializer.Deserialize<NetworkMessage>(data);
        }

        public byte[] Serialize(NetworkMessage msg)
        {
            return MessagePackSerializer.Serialize(msg, MessagePack.Resolvers.ContractlessStandardResolver.Instance);
        }

        private void Get(NetworkMessage networkMessage)
        {
            Global.Events.Raise(new NetworkReciveEvent() { Message = networkMessage.Data, Sender = networkMessage.Recipient }, networkMessage.Recipient);
        }

        private void Send(object data, string[] args)
        {
            if (args == default)
            {
                throw new Exception("Сообщение без получателя");
            }

            var msg = new NetworkMessage()
            {
                Recipient = args[0],
                Data = data
            };

            var bytes = Serialize(msg);

            var m = peer.CreateMessage();
            m.Write(bytes);

            if (IsServer == true)
            {
                var server = peer as NetServer;
                server.SendMessage(m, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
            }
            else if (IsServer == false)
            {
                var client = peer as NetClient;
                client.SendMessage(m, NetDeliveryMethod.ReliableOrdered);
            }
        }
    }
}