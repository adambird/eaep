using System;
using System.Globalization;
using System.IO;
using eaep.servicehost.store;
using Newtonsoft.Json;

namespace eaep.servicehost.http
{
    public class CountService : HttpService
    {
        IEAEPMonitorStore store;

        public CountService(IEAEPMonitorStore store)
        {
            this.store = store; 
        }

        public override void ProcessRequest(IServiceRequest request, IServiceResponse response, IResourceRepository resourceRepository)
        {
            CountResult[] result = store.Count(
                DateTime.ParseExact(request.GetParameter(Constants.QUERY_STRING_FROM), Constants.FORMAT_DATETIME, CultureInfo.InvariantCulture),
                DateTime.ParseExact(request.GetParameter(Constants.QUERY_STRING_TO), Constants.FORMAT_DATETIME, CultureInfo.InvariantCulture),
                int.Parse(request.GetParameter(Constants.QUERY_STRING_TIMESLICES)),
                request.GetParameter(Constants.QUERY_STRING_GROUPBY),
                request.Query
                );

            response.ContentType = Constants.CONTENT_TYPE_JSON;

            using (StreamWriter writer = new StreamWriter(response.ContentStream))
            {
                writer.Write((string) JsonConvert.SerializeObject(result));
            }
        }
    }
}
