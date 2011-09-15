using System;

namespace eaep.servicehost.http
{
    public abstract class HttpService : IRequestHandler
    {
        #region IRequestHandler Members

        public void Handle(IServiceRequest request, IServiceResponse response)
        {
            Handle(request, response, new ResourceRepository());
        }

        public void Handle(IServiceRequest request, IServiceResponse response, IResourceRepository resourceRepository)
        {
            try
            {
                ProcessRequest(request, response, resourceRepository);
                response.StatusCode = Constants.HTTP_200_OK;
                response.StatusDescription = "OK";
            }
            catch (Exception)
            {
                response.StatusCode = Constants.HTTP_500_SERVER_ERROR;
                response.StatusDescription = "Error, review logs";
            }

        }

        #endregion

        public abstract void ProcessRequest(IServiceRequest request, IServiceResponse response, IResourceRepository resourceRepository);
    }
}
