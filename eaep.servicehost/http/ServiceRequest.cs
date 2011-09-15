using System.IO;
using System.Linq;
using System.Net;

namespace eaep.servicehost.http
{
    public class ServiceRequest : IServiceRequest
    {
        private HttpListenerRequest request;

        protected string resourceName;
        protected string extension;
        protected string query;

        public ServiceRequest(HttpListenerRequest request)
        {
            LoadBaseRequest(request);
        }

        public HttpListenerRequest BaseRequest
        {
            get { return request; }
        }

        protected void LoadBaseRequest(HttpListenerRequest request)
        {
            LoadQuery(request);
            LoadResource(request);
            this.request = request;
        }

        protected void LoadQuery(HttpListenerRequest request)
        {
            this.query = request.QueryString[Constants.QUERY_STRING_QUERY];
        }

        protected void LoadResource(HttpListenerRequest request)
        {
            string localPath = request.Url.LocalPath;
            resourceName = localPath.Substring(localPath.LastIndexOf('/') + 1);

            string[] elements = resourceName.Split('.');
            this.extension = "." + elements.Last<string>();
        }

        #region IServiceRequest Members

        public string Query
        {
            get { return this.query; }
        }

        public string Extension
        {
            get { return this.extension; }
        }

        public string ResourceName
        {
            get { return this.resourceName; }
        }

        public string GetParameter(string name)
        {
            return this.request.QueryString[name];
        }

        public string Method
        {
            get { return this.request.HttpMethod; }
        }

        public string Body
        {
            get
            {
                using (StreamReader reader = new StreamReader(this.request.InputStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        #endregion
    }
}
