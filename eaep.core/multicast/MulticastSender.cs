using System.Net;
using System.Net.Sockets;

namespace eaep.multicast
{
    internal class MulticastSender : IMulticastSender
    {
        public readonly Socket socket;
        public readonly IPEndPoint endPoint;

        public MulticastSender(MulticastSettings settings)
        {
            endPoint = new IPEndPoint(settings.MulticastGroupAddress, settings.Port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public void Send(byte[] data)
        {
            socket.SendTo(data, endPoint);
        }
    }
}