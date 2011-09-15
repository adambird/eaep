using System;
using System.Net;

namespace eaep.multicast
{
    public class MulticastSettings
    {
        public int Port { get; set; }
        public int TimeToLive { get; set; }
        public IPAddress MulticastGroupAddress { get; set; }

        public MulticastSettings(string multicastGroupAddress, int port, int timeToLive)
        {
            IPAddress address;
            if (!IPAddress.TryParse(multicastGroupAddress, out address))
            {
                throw new ArgumentException("multicastGroupAddress was not a valid IP address", "multicastGroupAddress");
            }

            MulticastGroupAddress = address;
            Port = port;
            TimeToLive = timeToLive;
        }

        public override string ToString()
        {
            string messageFormat = "MulticastGroupAddress = [{0}], Port = [{1}], TimeToLive = [{2}]";
            return string.Format(messageFormat, MulticastGroupAddress, Port, TimeToLive);
        }
    }
}
