using System.IO;
using System.Net;

namespace eaep.servicehost.http
{
	public class HtmlPage : IRequestHandler
	{
        public void Render(HttpListenerResponse response)
        {
        }
        #region IRequestHandler Members

        public void Handle(IServiceRequest request, IServiceResponse response)
        {
            Handle(request, response, new ResourceRepository());
        }

        public void Handle(IServiceRequest request, IServiceResponse response, IResourceRepository resourceRepository)
        {
            WriteContent(request, response, resourceRepository);
            response.StatusCode = Constants.HTTP_200_OK;
            response.StatusDescription = "OK";
        }

        protected void WriteContent(IServiceRequest request, IServiceResponse response, IResourceRepository resourceRepository)
        {
           response.ContentType = "text/html";
           using (StreamWriter targetWriter = new StreamWriter(response.ContentStream))
            {
                InternalWriteContent(targetWriter, request, response, resourceRepository);
            }
        }
        /// <summary>
        /// Will write the resourcename as an html document. If sub class overrides this then 
        /// sub class method must specify the content type.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="resourceRepository"></param>
        protected virtual void InternalWriteContent(StreamWriter writer, IServiceRequest request, IServiceResponse response, IResourceRepository resourceRepository)
        {
            writer.Write(resourceRepository.GetResourceAsString(request.ResourceName));
        }

        #endregion

        protected string ReplaceCRLFwithHTMLLineBreaks(string source)
        {
            return source.Replace("\r\n", "<br/>");
        }
    }
}
