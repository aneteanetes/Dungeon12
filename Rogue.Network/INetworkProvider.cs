namespace Rogue.Network
{
    public interface INetworkProvider
    {
        void CreateServer();

        void ConnectServer(string ip, string port);

        void Send<TReciver>(object data);

        void Recive(object data);
    }
}