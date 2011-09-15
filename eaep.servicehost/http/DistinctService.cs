using System;
using System.Globalization;
using System.IO;
using eaep.servicehost.store;
using Newtonsoft.Json;

namespace eaep.servicehost.http
{
    public class DistinctService : HttpService
    {
        IEAEPMonitorStore store;

        public DistinctService(IEAEPMonitorStore store)
        {
            this.store = store;
        }

        public override void ProcessRequest(IServiceRequest request, IServiceResponse response, IResourceRepository resourceRepository)
        {
            string[] values = store.Distinct(
                request.GetParameter(Constants.QUERY_STRING_FIELD),
                DateTime.ParseExact(request.GetParameter(Constants.QUERY_STRING_FROM), Constants.FORMAT_DATETIME, CultureInfo.InvariantCulture),
                DateTime.ParseExact(request.GetParameter(Constants.QUERY_STRING_TO), Constants.FORMAT_DATETIME, CultureInfo.InvariantCulture),
                request.Query
                );

            response.ContentType = Constants.CONTENT_TYPE_JSON;

            using (StreamWriter writer = new StreamWriter(response.ContentStream))
            {
                string json = JsonConvert.SerializeObject(values);
                writer.Write(json);
            }

        }
    }
}
