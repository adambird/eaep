using System;
using System.Collections.Generic;
using System.Text;

namespace eaep.multicast
{
    public delegate void ReceiveHandler(byte[] data);

    public interface IMulticast
    {
        MulticastSettings Settings { get; }

        event ReceiveHandler DataReceived;

        void Start();
        void Stop();

        void Broadcast(byte[] data);


    }
}
