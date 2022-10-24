using Lidgren.Network;
//using MessagePack;
using Dungeon.Events.Network;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Dungeon.Network
{
    public partial class GameNetwork
    {
        private static GameNetwork instance;

        public static void Start()
        {
            instance = new GameNetwork();
        }

        private bool? IsServer = false;
        private NetPeer peer;

        internal GameNetwork()
        {
            Console.WriteLine(instance);

            DungeonGlobal.Events.Subscribe<CreateNetworkServerEvent, GameNetwork>(StartSever);
            DungeonGlobal.Events.Subscribe<ConnectNetworkServerEvent, GameNetwork>(StartClient);
            DungeonGlobal.Events.Subscribe<NetworkSendEvent, GameNetwork>(Send);
        }

        //private readonly MessagePackSerializer serializer = MessagePackSerializer.Get<NetworkMessage>();

        private GameNetworkMessage Deserialize(byte[] data)
        {
            //return (NetworkMessage)serializer.Unpack(new MemoryStream(data));

#warning MessagePack в данный момент потенциально опасен, а сеть не работает, поэтому пусть так
            return default;

            //var msg = MessagePackSerializer.Deserialize<NetworkMessage>(data, MessagePack.Resolvers.ContractlessStandardResolver.Instance);

            //var dataType = Type.GetType(msg.DataType);
            //if (IsSimple(dataType))
            //{
            //    msg.Data = Convert.ChangeType(msg.Data, dataType);
            //}
            //else
            //{
            //    msg.Data = msg.DataType.CreateAndFill(msg.Data as IDictionary<object, object>);
            //}

            //return msg;
        }

        bool IsSimple(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(typeInfo.GetGenericArguments()[0]);
            }
            return typeInfo.IsPrimitive
              || typeInfo.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }

        private byte[] Serialize(GameNetworkMessage msg)
        {

#warning MessagePack в данный момент потенциально опасен, а сеть не работает, поэтому пусть так
            return default;
            //return MessagePackSerializer.Serialize(msg, MessagePack.Resolvers.ContractlessStandardResolver.Instance);
        }

        private void Get(GameNetworkMessage networkMessage)
        {
            DungeonGlobal.Events.Raise(new NetworkReciveEvent() { Message = networkMessage.Data, Sender = networkMessage.Recipient }, networkMessage.Recipient);
        }

        private void Send(object data, string[] args)
        {
            if (peer == null)
            {
                return;
            }

            if (args == default)
            {
                throw new Exception("Сообщение без получателя");
            }

            var msg = new GameNetworkMessage()
            {
                Recipient = args[0],
                Data = data.GetProperty("Message"),
                DataType = data.GetProperty("Message").GetType().AssemblyQualifiedName
            };

            var bytes = Serialize(msg);

            var m = peer.CreateMessage();
            m.Write(bytes);

            if (IsServer == true)
            {
                var server = peer as NetServer;
                if (server.ConnectionsCount > 0)
                {
                    server.SendMessage(m, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                }
            }
            else if (IsServer == false)
            {
                var client = peer as NetClient;
                client.SendMessage(m, NetDeliveryMethod.ReliableOrdered);
            }
        }
    }
}