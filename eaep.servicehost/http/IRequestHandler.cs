namespace eaep.servicehost.http
{
    public interface IRequestHandler
    {
        void Handle(IServiceRequest request, IServiceResponse response);
        void Handle(IServiceRequest request, IServiceResponse response, IResourceRepository resourceRepository);
    }
}
