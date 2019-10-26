using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Network
{
    public class NetworkMessage
    {
        public string Recipient { get; set; }

        public object Data { get; set; }
    }
}
