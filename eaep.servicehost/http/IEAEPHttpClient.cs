namespace eaep.servicehost.http
{
    public interface IEAEPHttpClient
    {
        void SendMessage(string uri, EAEPMessage message, EndSendEAEPMessageDelegate onwardDelegate);
        void SendMessage(string uri, EAEPMessage message);
    }
}
