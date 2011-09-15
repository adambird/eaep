namespace eaep.servicehost.http
{
    public interface IServiceRequest
    {
        string Query { get; }
        string Extension { get; }
        string ResourceName { get; }
        string GetParameter(string name);
        string Method { get; }
        string Body { get; }
    }
}
