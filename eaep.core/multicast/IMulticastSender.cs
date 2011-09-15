namespace eaep.multicast
{
    internal interface IMulticastSender
    {
        void Send(byte[] data);
    }
}