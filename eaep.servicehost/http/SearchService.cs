using System;
using System.Globalization;
using System.IO;
using eaep.servicehost.store;
using Newtonsoft.Json;

namespace eaep.servicehost.http
{
    public class SearchService : HttpService
    {
        IEAEPMonitorStore monitor;

        public SearchService(IEAEPMonitorStore monitor)
        {
            this.monitor = monitor;
        }

        public override void ProcessRequest(IServiceRequest request, IServiceResponse response, IResourceRepository resourceRepository)
        {
            response.ContentType = Constants.CONTENT_TYPE_JSON;

            EAEPMessages messages = null;

            if (request.GetParameter(Constants.QUERY_STRING_FROM) != null)
            {
                DateTime from = DateTime.ParseExact(request.GetParameter(Constants.QUERY_STRING_FROM), Constants.FORMAT_DATETIME, CultureInfo.InvariantCulture);
                if (request.GetParameter(Constants.QUERY_STRING_TO) != null)
                {
                    messages = monitor.GetMessages(
                        from,
                        DateTime.ParseExact(request.GetParameter(Constants.QUERY_STRING_TO), Constants.FORMAT_DATETIME, CultureInfo.InvariantCulture),
                        request.Query
                        );
                }
                else
                {
                    messages = monitor.GetMessages(from, request.Query);
                }
            }
            else
            {
                messages = monitor.GetMessages(request.Query);
            }
            using (StreamWriter writer = new StreamWriter(response.ContentStream))
            {
                string json = JsonConvert.SerializeObject(messages);
                writer.Write(json);
            }

        }
    }
}
