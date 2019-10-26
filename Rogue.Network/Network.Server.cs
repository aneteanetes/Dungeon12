using Lidgren.Network;
using Rogue.Events.Network;
using System.Threading;

namespace Rogue.Network
{
    internal partial class Network
    {
        private void StartSever(CreateNetworkServerEvent e)
        {
            IsServer = true;
            var config = new NetPeerConfiguration("Dungeon 12 v0.5")
            {
                Port = 3444
            };
            var server = new NetServer(config);
            peer = server;
            server.Start();
            server.RegisterReceivedCallback(new SendOrPostCallback(ServerMessage));
        }

        public void ServerMessage(object peer)
        {
            var message = (peer as NetServer).ReadMessage();

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
                    int datalen = message.ReadInt32();
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
