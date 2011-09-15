using System.IO;
using System.Net;

namespace eaep.servicehost.http
{
    public class ServiceResponse : IServiceResponse
    {
        protected HttpListenerResponse response;

        public ServiceResponse(HttpListenerResponse response)
        {
            this.response = response;
        }

        #region IServiceResponse Members

        public string ContentType
        {
            get { return this.response.ContentType; }
            set { this.response.ContentType = value; }
        }

        public Stream ContentStream
        {
            get { return this.response.OutputStream; }
        }

        public int StatusCode
        {
            get { return this.response.StatusCode; }
            set { this.response.StatusCode = value; }
        }

        public string StatusDescription
        {
            get { return this.response.StatusDescription; }
            set { this.response.StatusDescription = value; }
        }

        #endregion
    }
}
