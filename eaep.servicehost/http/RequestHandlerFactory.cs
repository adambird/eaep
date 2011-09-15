using eaep.servicehost.store;

namespace eaep.servicehost.http
{
    public class RequestHandlerFactory
    {
        public static IRequestHandler GetHandler(IServiceRequest request, IEAEPMonitorStore monitorStore)
        {
            switch (request.Extension.ToLower())
            {
                case ".json":
                    switch (request.ResourceName.ToLower())
                    {
                        case "count.json":
                            return new CountService(monitorStore);
                        case "distinct.json":
                            return new DistinctService(monitorStore);
                        default:
                            return new SearchService(monitorStore);
                    }
                default:
                    switch (request.ResourceName.ToLower())
                    {
                        case "event":
                            return new EventService(monitorStore);
                        default:
                            return new SearchPage(monitorStore);
                    }
            }
        }
    }
}
