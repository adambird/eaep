namespace eaep
{
    public interface IEAEPMessageSerializer
    {
        EAEPMessage Deserialize(byte[] data);
    }
}