using System.IO;

namespace eaep.servicehost.http
{
    public interface IResourceRepository
    {
        void WriteResource(string resourceName, StreamWriter writer);
        void WriteResource(string resourceName, Stream stream);
        string GetResourceAsString(string resourceName);
    }
}
