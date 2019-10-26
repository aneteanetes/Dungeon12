using Rogue.Events.Network;
using Rogue.Proxy;

namespace Rogue.Network
{
    /// <summary>
    /// Для получения используется динамический диспатч для принятия разных типов сообщений
    /// </summary>
    public class NetObject : ProxyObject
    {
        /// <summary>
        /// Начать взаимодействовать по сети
        /// </summary>
        public void Network() => Global.Events.Subscribe<NetworkReciveEvent>(e => this.Dispatch((x, v) => x.Recive(v), e.Message), false, ProxyId);

        protected void Send<T>(T value) => Global.Events.Raise(new NetworkSendEvent(value) { Recipient = ProxyId }, this.ProxyId);

        public virtual bool Recive(object @object)
        {
            return this.CallRecive(@object as dynamic);
        }

        protected virtual bool CallRecive(dynamic obj)
        {
            return Recive(obj);
        }

        public virtual void Send(object @object)
        {
            this.CallSend(@object as dynamic);
        }

        protected virtual void CallSend(dynamic obj)
        {
            Recive(obj);
        }
    }
}