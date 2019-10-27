using Lidgren.Network;
using Rogue.Events.Network;
using System.Threading;

namespace Rogue.Network
{
    public partial class Network
    {
        private void StartClient(object e, string[] args)
        {
            IsServer = false;
            var config = new NetPeerConfiguration("Dungeon 12 v0.5");
            var client = new NetClient(config);
            client.Start();
            client.Connect(host: "192.168.137.242", port: 3444);
            peer = client;
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            client.RegisterReceivedCallback(new SendOrPostCallback(ClientMessage));
        }

        private void ClientMessage(object peer)
        {
            var message = (peer as NetClient).ReadMessage();

            switch (message.MessageType)
            {
                case NetIncomingMessageType.Error:
                    break;
                case NetIncomingMessageType.StatusChanged:
                    break;
                case NetIncomingMessageType.UnconnectedData:
                    break;
                case NetIncomingMessageType.ConnectionApproval:
                    break;
                case NetIncomingMessageType.Data:
                    int datalen = message.LengthBytes;
                    byte[] data = message.ReadBytes(datalen);
                    var msg = Deserialize(data);
                    Get(msg);
                    break;
                case NetIncomingMessageType.Receipt:
                    break;
                case NetIncomingMessageType.DiscoveryRequest:
                    break;
                case NetIncomingMessageType.DiscoveryResponse:
                    break;
                case NetIncomingMessageType.VerboseDebugMessage:
                    break;
                case NetIncomingMessageType.DebugMessage:
                    break;
                case NetIncomingMessageType.WarningMessage:
                    break;
                case NetIncomingMessageType.ErrorMessage:
                    break;
                case NetIncomingMessageType.NatIntroductionSuccess:
                    break;
                case NetIncomingMessageType.ConnectionLatencyUpdated:
                    break;
                default:
                    break;
            }
        }
    }
}
