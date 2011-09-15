using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace eaep.multicast
{
    public class StateObject
    {
        public Socket Socket;

        //  64k
        public byte[] Buffer = new byte[ushort.MaxValue];
    }
}
