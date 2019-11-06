using Dungeon.Events.Network;
using System.Collections.Generic;

namespace Dungeon.Network
{
    public class NetProxy : ProxyProperty
    {
        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public override T Get<T>(T v, string proxyId)
        {
            //if (!___GetCache.Contains(proxyId))
            //{
            //    Global.Events.Subscribe<NetworkReciveEvent>(e =>
            //    {
            //        this.__Set(e.Message.As<T>());
            //    }, false, proxyId);

            //    ___GetCache.Add(proxyId);
            //}

            return v;
        }

        private static readonly HashSet<string> ___GetCache = new HashSet<string>();
        
        public override T Set<T>(T v, string proxyId)
        {
            //Global.Events.Raise(new NetworkSendEvent(v) { Recipient = proxyId }, proxyId);
            return v;
        }
    }
}
