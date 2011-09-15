using System.IO;

namespace eaep.servicehost.http
{
    public interface IServiceResponse
    {
        string ContentType { get;  set; }
        Stream ContentStream { get; }
        int StatusCode { get; set; }
        string StatusDescription { get; set; }
    }
}
